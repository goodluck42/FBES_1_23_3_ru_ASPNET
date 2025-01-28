namespace ToDoList.Services;

[Obsolete]
public sealed class OffsetPagination<T>(IEnumerable<T> entities) : IOffsetPagination<T>
	where T : class
{
	public IOffsetPagination<T> Take(int count)
	{
		return new OffsetPagination<T>(entities.Take(count));
	}

	public IOffsetPagination<T> Skip(int count)
	{
		return new OffsetPagination<T>(entities.Skip(count));
	}

	public IEnumerable<T> GetResult()
	{
		return entities;
	}
}