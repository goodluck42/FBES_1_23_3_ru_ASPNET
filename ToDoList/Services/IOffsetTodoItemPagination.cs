using ToDoList.Entity;

namespace ToDoList.Services;

public interface IOffsetTodoItemPagination
{
	Task<IEnumerable<ToDoItem>> GetAsync(PaginationSegment paginationSegment);
}