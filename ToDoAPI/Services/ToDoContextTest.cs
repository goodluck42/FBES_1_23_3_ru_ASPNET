using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Entity;
using ToDoAPI.Exceptions;

namespace ToDoAPI.Services;

public sealed class ToDoContextTest : ToDoContextBase, IOffsetTodoItemPagination, IToDoItemSorter
{
	private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

	public ToDoContextTest(IDbContextFactory<AppDbContext> dbContextFactory)
	{
		_dbContextFactory = dbContextFactory;
#if DEBUG
		Console.WriteLine(Id);
#endif
	}
#if DEBUG
	public Guid Id { get; } = Guid.NewGuid();
#endif

	public override async Task<ToDoItem> AddAsync(ToDoItem item)
	{
		var dbContext = await _dbContextFactory.CreateDbContextAsync();

		await using (dbContext)
		{
			item.Version = Guid.NewGuid();

			var result = await dbContext.ToDoItems.AddAsync(item);

			await dbContext.SaveChangesAsync();

			return result.Entity;
		}
	}

	public override async Task RemoveAsync(int id)
	{
		await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

		dbContext.ToDoItems.Remove(new ToDoItem
		{
			Id = id
		});

		await dbContext.SaveChangesAsync();
	}

	public override async Task UpdateAsync(int id, ToDoItem item)
	{
		await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

		var originalItem = await dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == id);

		if (originalItem is null)
		{
			throw new ToDoItemNotFoundException();
		}

		originalItem.Title = item.Title;
		originalItem.Description = item.Description;
		originalItem.Priority = item.Priority;
		originalItem.CompletionDateTime = item.CompletionDateTime;
		originalItem.Version = Guid.NewGuid();

		await dbContext.SaveChangesAsync();
	}

	public override async Task<IEnumerable<ToDoItem>> GetAsync()
	{
		await using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
			return await dbContext.ToDoItems.ToListAsync();
	}

	public override async Task<ToDoItem> GetAsync(int id)
	{
		await using (var dbContext = await _dbContextFactory.CreateDbContextAsync())
			return dbContext.ToDoItems.FirstOrDefault(x => x.Id == id) ?? throw new ToDoItemNotFoundException();
	}

	public async Task<IEnumerable<ToDoItem>> GetAsync(PaginationSegment paginationSegment)
	{
		await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

		return await dbContext.ToDoItems.Skip(paginationSegment.Offset).Take(paginationSegment.Count).ToListAsync();
	}

	public async Task<IEnumerable<ToDoItem>> SortBy(ToDoItemSortOption option, bool isAscending = true,
		PaginationSegment? paginationSegment = null)
	{
		await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

		if (paginationSegment is null)
		{
			paginationSegment = new PaginationSegment(Constants.DefaultOffset, Constants.DefaultTake);
		}

		return option switch
		{
			ToDoItemSortOption.Priority => OrderByAndPaginate(x => x.Priority),
			ToDoItemSortOption.CompletionDateTime => OrderByAndPaginate(x => x.CompletionDateTime),
			_ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
		};

		IEnumerable<ToDoItem> OrderByAndPaginate<TKey>(Func<ToDoItem, TKey> keySelector)
		{
			return (isAscending
					? dbContext.ToDoItems.OrderBy(keySelector)
					: dbContext.ToDoItems.OrderByDescending(keySelector))
				.Skip(paginationSegment.Value.Offset)
				.Take(paginationSegment.Value.Count).ToList();
		}
	}
}