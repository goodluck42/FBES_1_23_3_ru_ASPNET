using System.Text.Json;
using AutoMapper;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.Security;
using ToDoAPI.Services;

namespace ToDoAPI.EndpointMappers;

// ABAC - Attribute Based Access Control

public class ToDoEndpointMapper : IEndpointMapper
{
	public static void Map(IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api/todos");
		
		//// versioning
		// api/v1
		// api/v2
		//// HTTP methods

		//// GET
		// api/v1/users?count=10&offset=5 - GET all users
		// api/v1/users/{id:int} - GET user by id
		// api/v1/users/login/{login:str} - GET user by login

		//// POST
		// api/v1/users - POST add new user

		//// PATCH
		// api/v1/users/{id}/edit - PATCH partially update user 

		//// PUT & DELETE
		// api/v1/users/{id} - PUT or DELETE replace or delete a user 

		// endpoints.MapGet("/api/v1/my_todos", (int? offset, int? count = 10) =>
		// {
		// 	Console.WriteLine("Endpoint");
		//
		// 	return ResultsApi.Json(new
		// 	{
		// 		Offset = offset,
		// 		Count = count,
		// 		OK = true
		// 	});
		// }).AddEndpointFilter<ValidationFilter>();

		group.MapPost("/", async (IToDoContext context, Mapper mapper, ToDoItemDto? toDoItemDto) =>
		{
			if (toDoItemDto is null)
			{
				return ResultsApi.BadRequest();
			}

			var addedItem = await context.AddAsync(mapper.Map<ToDoItem>(toDoItemDto));

			return ResultsApi.Created($"/todos/{addedItem.Id}", addedItem);
		});

		group.MapGet("/{id:int}",
				async (IToDoContext context, int id) => ResultsApi.Json(await context.GetAsync(id)))
			.AddEndpointFilter(
				async (ctx, next) =>
				{
					var id = ctx.GetArgument<int>(1);

					if (id < -1000)
					{
						return ResultsApi.BadRequest("Invalid Id");
					}

					return await next(ctx);
				});

		// api/v1/todos?offset=5&count=10
		group.MapGet("/{offset:int}/{count:int}", async (
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
				return ResultsApi.Json(await pagination.GetAsync(new PaginationSegment(offset, count)));
			}

			return ResultsApi.Json(await sorter.SortBy(sortBy.Value, isAscending, new PaginationSegment(offset, count)));
		});

		group.MapGet("/dump", async (IToDoContext context, HttpContext httpContext) =>
		{
			var data = await context.GetAsync();
			var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(data, new JsonSerializerOptions
			{
				WriteIndented = true
			});

			return ResultsApi.File(jsonBytes, "application/json", "dump");
		});

		group.MapPost("/attachImage/{id:int}", async (int id, HttpContext context) =>
		{
			var formFile = (await context.Request.ReadFormAsync()).Files["file"];

			if (formFile is null)
			{
				return ResultsApi.BadRequest();
			}

			var exts = MimeTypes.GetMimeTypeExtensions(formFile.ContentType);
			var path = $"{id}.{exts.First()}";

			await using var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

			await formFile.OpenReadStream().CopyToAsync(fileStream);

			return ResultsApi.Ok();
		});

		group.RequireAuthorization(Policies.UserPolicy);
	}
}

file sealed class ValidationFilter(IConfiguration configuration) : IEndpointFilter
{
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		Console.WriteLine($"Filter: {configuration.GetJwtIssuer()}");

		var offset = context.GetArgument<int?>(0);
		var count = context.GetArgument<int?>(1);

		if (offset is null or < 0 || count is null or < 0)
		{
			return ResultsApi.BadRequest("Validation failed");
		}

		return await next(context);
	}
}