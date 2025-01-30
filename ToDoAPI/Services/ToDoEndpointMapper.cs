using AutoMapper;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

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

		group.MapGet("/{offset:int}/{count:int}", async (
			IOffsetTodoItemPagination pagination,
			IToDoItemSorter sorter,
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
	}
}