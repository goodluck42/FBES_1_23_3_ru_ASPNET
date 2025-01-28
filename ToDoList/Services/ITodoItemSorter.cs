using ToDoList.Entity;

namespace ToDoList.Services;

public interface ITodoItemSorter
{
	Task<IEnumerable<ToDoItem>> SortBy(ToDoItemSortOption option, bool isAscending = true,
		PaginationSegment? paginationSegment = null);
}