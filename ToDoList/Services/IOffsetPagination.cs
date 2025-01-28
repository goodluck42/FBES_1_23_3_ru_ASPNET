using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

namespace ToDoList.Services;

[Obsolete]
public interface IOffsetPagination<out T>
	where T : class
{
	IOffsetPagination<T> Take(int count);
	IOffsetPagination<T> Skip(int count);

	IEnumerable<T> GetResult();
}