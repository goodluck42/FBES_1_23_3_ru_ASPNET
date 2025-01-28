using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dtos;
using ToDoList.Entity;
using ToDoList.Services;

namespace ToDoList.Controllers;

public class ToDoController(
	IToDoContext toDoContext,
	Mapper mapper,
	IOffsetTodoItemPagination pagination,
	ITodoItemSorter sorter)
	: Controller
{
	public async Task<IActionResult> Index()
	{
		return View(nameof(List), model: await toDoContext.GetAsync());
	}

	[HttpPost]
	[HttpGet]
	public async Task<IActionResult> Create(ToDoItemDto? toDoItemDto)
	{
		if (HttpContext.Request.Method == HttpMethods.Get)
		{
			return View();
		}

		if (HttpContext.Request.Method == HttpMethods.Post)
		{
			if (toDoItemDto is null)
			{
				return BadRequest();
			}

			await toDoContext.AddAsync(mapper.Map<ToDoItem>(toDoItemDto));

			return RedirectToAction(nameof(Index));
		}

		throw new InvalidOperationException();
	}

	[HttpGet]
	public async Task<IActionResult> List(int offset = Constants.DefaultOffset, int count = Constants.DefaultTake,
		ToDoItemSortOption? sortBy = null,
		bool isAscending = true)
	{
		if (sortBy is null)
		{
			return View(model: await pagination.GetAsync(new PaginationSegment(offset, count)));
		}

		return View(model: await sorter.SortBy(sortBy.Value, isAscending, new PaginationSegment(offset, count)));
	}

	// todo/details?id=1 
	// todo/details/1 
	[HttpGet]
	public async Task<IActionResult> Details(int? id)
	{
		if (id is null)
		{
			return BadRequest();
		}

		return View(model: await toDoContext.GetAsync(id.Value));
	}

	[HttpPost]
	public async Task<IActionResult> Update(ToDoItem? toDoItem)
	{
		if (toDoItem is not null)
		{
			await toDoContext.UpdateAsync(toDoItem);

			return RedirectToAction(nameof(List));
		}

		return BadRequest();
	}

	[HttpGet]
	public async Task<IActionResult> Update(int? id)
	{
		if (id is not null)
		{
			var toDoItem = await toDoContext.GetAsync(id.Value);

			return View(toDoItem);
		}

		return BadRequest();
	}
}