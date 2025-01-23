using DI_MVC.Models;

namespace DI_MVC.Services;

public sealed class StockStorage : IStockStorage
{
	private readonly List<StockItem> _items = [];

	public void Add(StockItem item)
	{
		_items.Add(item);
	}

	public IEnumerable<StockItem> GetAll()
	{
		return _items;
	}
}