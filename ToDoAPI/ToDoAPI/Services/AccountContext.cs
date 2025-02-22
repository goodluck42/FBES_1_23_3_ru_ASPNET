using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public class AccountContext(IDbContextFactory<AppDbContext> dbContextFactory) : IAccountContext
{
	public async Task AddAsync(Account account)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		dbContext.Accounts.Add(account);

		await dbContext.SaveChangesAsync();
	}

	public async Task RemoveAsync(int id)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		dbContext.Accounts.Remove(new Account { Id = id });

		await dbContext.SaveChangesAsync();
	}

	public async Task<Account> GetAsync(string login)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		return dbContext.Accounts.First(a => a.Login == login);
	}

	public async Task<int> CountAsync()
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		return await dbContext.Accounts.CountAsync();
	}

	public async Task<IEnumerable<Role>> GetRolesAsync(int accountId)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		return dbContext.Accounts.Include(x => x.Roles).First(x => x.Id == accountId).Roles;
	}
}