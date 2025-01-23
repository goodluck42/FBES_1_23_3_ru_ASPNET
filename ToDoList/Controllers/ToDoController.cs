using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Dtos;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList.Controllers;

public class ToDoController([FromKeyedServices(ServiceStaticKeys.ToDoService)] IToDoContext toDoContext, Mapper mapper)
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
	public async Task<IActionResult> List()
	{
		return View(model: await toDoContext.GetAsync());
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


	[HttpGet] // todo/widget
	public async Task<IActionResult> Widget()
	{
		return PartialView("Widget");
	}
}