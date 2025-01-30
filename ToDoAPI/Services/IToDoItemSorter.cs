using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IToDoItemSorter
{
	Task<IEnumerable<ToDoItem>> SortBy(ToDoItemSortOption option, bool isAscending = true,
		PaginationSegment? paginationSegment = null);
}