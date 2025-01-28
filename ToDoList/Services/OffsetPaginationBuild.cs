namespace ToDoList.Services;

[Obsolete]
public sealed class OffsetPaginationBuild : IOffsetPaginationBuild
{
	public IOffsetPagination<TEntity> SelectEntity<TEntity>(Func<IEnumerable<TEntity>> func)
		where TEntity : class
	{
		return new OffsetPagination<TEntity>(func());
	}
}