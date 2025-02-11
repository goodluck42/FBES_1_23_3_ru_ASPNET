using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public class RefreshTokenManager(IDbContextFactory<AppDbContext> dbContextFactory) : IRefreshTokenManager
{
	public async Task AddOrUpdate(RefreshToken refreshToken)
	{
		using var dbContext = dbContextFactory.CreateDbContext();

		var token = dbContext.RefreshTokens.FirstOrDefault(x => x.Id == refreshToken.Id);

		if (token is null)
		{
			dbContext.RefreshTokens.Add(refreshToken);
		}
		else
		{
			dbContext.RefreshTokens.Update(refreshToken);
		}

		await dbContext.SaveChangesAsync();
	}

	public async Task Add(RefreshToken refreshToken)
	{
		using var dbContext = dbContextFactory.CreateDbContext();

		dbContext.RefreshTokens.Add(refreshToken);

		await dbContext.SaveChangesAsync();
	}

	public async Task Update(RefreshToken refreshToken)
	{
		using var dbContext = dbContextFactory.CreateDbContext();

		dbContext.RefreshTokens.Update(refreshToken);

		await dbContext.SaveChangesAsync();
	}

	public async Task Remove(RefreshToken refreshToken)
	{
		using var dbContext = dbContextFactory.CreateDbContext();

		dbContext.RefreshTokens.Remove(refreshToken);

		await dbContext.SaveChangesAsync();
	}

	public async Task<RefreshToken> GetByAccountId(int accountId)
	{
		using var dbContext = dbContextFactory.CreateDbContext();

		return await dbContext.RefreshTokens.FirstAsync(x => x.AccountId == accountId);
	}
}