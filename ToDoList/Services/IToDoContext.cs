using ToDoList.Entity;

namespace ToDoList.Services;

public interface IToDoContext
{
	Task AddAsync(ToDoItem item);
	Task RemoveAsync(int id);
	Task RemoveAsync(ToDoItem item);
	Task UpdateAsync(int id, ToDoItem item);
	Task UpdateAsync(ToDoItem item);
	Task<IEnumerable<ToDoItem>> GetAsync();
	Task<ToDoItem> GetAsync(int id);
}