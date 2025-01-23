using DI_MVC.Models;
using DI_MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DI_MVC.Controllers;

public class StockController : Controller
{
	private readonly IStockStorage _stockStorage;
	
	public StockController(IServiceProvider provider) // dependency injection
	{
		_stockStorage = provider.GetRequiredService<IStockStorage>(); // locator
	}
	
	[HttpGet]
	public IActionResult Index()
	{
		// ...
		return View(_stockStorage.GetAll());
	}

	[HttpPost]
	public IActionResult AddItem(StockItem item)
	{
		_stockStorage.Add(item);

		return RedirectToAction("Index");
	}
}