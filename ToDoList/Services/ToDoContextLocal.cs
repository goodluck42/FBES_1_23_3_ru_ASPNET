using ToDoList.Entity;
using ToDoList.Exceptions;

namespace ToDoList.Services;

public sealed class ToDoContextLocal : ToDoContextBase
{
	private readonly List<ToDoItem> _items;
	private int _primaryKey = 1;

	public ToDoContextLocal()
	{
		_items = [];
	}

	public override Task AddAsync(ToDoItem item)
	{
		// if (_items.Any(x => x.Id == item.Id))
		// {
		// 	throw new DuplicateToDoItemException();
		// }

		item.Id = _primaryKey++;
		
		_items.Add(item);

		return Task.CompletedTask;
	}

	public override Task RemoveAsync(int id)
	{
		var index = _items.FindIndex(x => x.Id == id);

		if (index == -1)
		{
			throw new ToDoItemNotFoundException();
		}

		_items.RemoveAt(index);

		return Task.CompletedTask;
	}

	public override Task UpdateAsync(int id, ToDoItem item)
	{
		var index = _items.FindIndex(x => x.Id == id);

		if (index == -1)
		{
			throw new ToDoItemNotFoundException();
		}

		_items[index] = item;
		item.Id = id;

		return Task.CompletedTask;
	}

	public override Task<IEnumerable<ToDoItem>> GetAsync() => Task.FromResult<IEnumerable<ToDoItem>>(_items);

	public override Task<ToDoItem> GetAsync(int id)
	{
		return Task.FromResult(_items.FirstOrDefault(x => x.Id == id) ?? throw new ToDoItemNotFoundException());
	}
}