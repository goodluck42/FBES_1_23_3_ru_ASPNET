namespace ToDoList.Services;

public interface IOffsetPaginationBuild
{
	IOffsetPagination<TEntity> SelectEntity<TEntity>(Func<IEnumerable<TEntity>> func)
		where TEntity : class;
}