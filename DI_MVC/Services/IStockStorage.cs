using DI_MVC.Models;

namespace DI_MVC.Services;

public interface IStockStorage
{
	void Add(StockItem item);
	IEnumerable<StockItem> GetAll();
}