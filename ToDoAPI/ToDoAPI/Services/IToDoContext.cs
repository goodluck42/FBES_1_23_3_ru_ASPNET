using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IToDoContext
{
	Task<ToDoItem> AddAsync(ToDoItem item);
	Task RemoveAsync(int id);
	Task RemoveAsync(ToDoItem item);
	Task UpdateAsync(int id, ToDoItem item);
	Task UpdateAsync(ToDoItem item);
	Task<IEnumerable<ToDoItem>> GetAsync();
	Task<ToDoItem> GetAsync(int id);
}