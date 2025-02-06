using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IOffsetTodoItemPagination
{
	Task<IEnumerable<ToDoItem>> GetAsync(PaginationSegment paginationSegment);
}