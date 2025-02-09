using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

// ABAC - Attribute Based Access Control

public class ToDoEndpointMapper : IEndpointMapper
{
	public static void Map(IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api/todos");

		group.MapPut("/", async (IToDoContext context, ToDoItem? toDoItem) =>
		{
			if (toDoItem is null)
			{
				return Results.BadRequest();
			}

			await context.UpdateAsync(toDoItem);

			return Results.NoContent();
		});

		group.MapPost("/", async (IToDoContext context, Mapper mapper, ToDoItemDto? toDoItemDto) =>
		{
			if (toDoItemDto is null)
			{
				return Results.BadRequest();
			}

			var addedItem = await context.AddAsync(mapper.Map<ToDoItem>(toDoItemDto));

			return Results.Created($"/todos/{addedItem.Id}", addedItem);
		});

		group.MapGet("/{id:int}",
			async (IToDoContext context, int id) => Results.Json(await context.GetAsync(id)));

		group.MapGet("/{offset:int}/{count:int}", [Authorize] async (
			IOffsetTodoItemPagination pagination,
			IToDoItemSorter sorter,
			HttpContext context,
			int offset = Constants.DefaultOffset,
			int count = Constants.DefaultTake,
			ToDoItemSortOption? sortBy = null,
			bool isAscending = true) =>
		{
			if (sortBy is null)
			{
				return Results.Json(await pagination.GetAsync(new PaginationSegment(offset, count)));
			}

			return Results.Json(await sorter.SortBy(sortBy.Value, isAscending, new PaginationSegment(offset, count)));
		});

		group.MapGet("/dump", async (IToDoContext context) =>
		{
			var data = await context.GetAsync();
			var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions
			{
				WriteIndented = true
			});

			return Results.File(jsonBytes, "application/json", "dump");
		});

		group.MapPost("/attachImage/{id:int}", async (int id, HttpContext context) =>
		{
			var formFile = (await context.Request.ReadFormAsync()).Files["file"];

			if (formFile is null)
			{
				return Results.BadRequest();
			}

			var exts = MimeTypes.GetMimeTypeExtensions(formFile.ContentType);
			var path = $"{id}.{exts.First()}";

			await using var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

			await formFile.OpenReadStream().CopyToAsync(fileStream);

			return Results.Ok();
		});
	}
}