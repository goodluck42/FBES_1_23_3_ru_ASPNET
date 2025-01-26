using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Entity;
using ToDoList.Exceptions;

namespace ToDoList.Services;

public sealed class ToDoContextTest(IDbContextFactory<AppDbContext> dbContextFactory) : ToDoContextBase
{
	public override async Task AddAsync(ToDoItem item)
	{
		var dbContext = await dbContextFactory.CreateDbContextAsync();

		await using (dbContext)
		{
			item.Version = Guid.NewGuid();

			await dbContext.ToDoItems.AddAsync(item);
			await dbContext.SaveChangesAsync();
		}
	}

	public override async Task RemoveAsync(int id)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		dbContext.ToDoItems.Remove(new ToDoItem
		{
			Id = id
		});

		await dbContext.SaveChangesAsync();
	}

	public override async Task UpdateAsync(int id, ToDoItem item)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
		// TODO: Possible govnokod
		await using (var dbContext = await dbContextFactory.CreateDbContextAsync()) return dbContext.ToDoItems;
	}

	public override async Task<ToDoItem> GetAsync(int id)
	{
		await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
			return dbContext.ToDoItems.FirstOrDefault(x => x.Id == id) ?? throw new ToDoItemNotFoundException();
	}
}

