using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public abstract class ToDoContextBase : IToDoContext
{
	public abstract Task<ToDoItem> AddAsync(ToDoItem item);
	public abstract Task RemoveAsync(int id);

	public Task RemoveAsync(ToDoItem item)
	{
		return RemoveAsync(item.Id);
	}

	public abstract Task UpdateAsync(int id, ToDoItem item);

	public Task UpdateAsync(ToDoItem item)
	{
		return UpdateAsync(item.Id, item);
	}

	public abstract Task<IEnumerable<ToDoItem>> GetAsync();
	public abstract Task<ToDoItem> GetAsync(int id);
}