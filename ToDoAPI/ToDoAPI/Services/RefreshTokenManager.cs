using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public class RefreshTokenManager(
	IDbContextFactory<AppDbContext> dbContextFactory,
	IRefreshTokenGenerator refreshTokenGenerator) : IRefreshTokenManager
{
	// boilerplate code
	public async Task<RefreshToken> AssignTokenAsync(Account account)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		var entry = dbContext.RefreshTokens.Add(new RefreshToken
		{
			AccountId = account.Id,
			Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(account),
			Expires = DateTime.Now.AddMonths(12),
		});

		await dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	public Task<RefreshToken> AssignTokenAsync(int accountId)
	{
		return AssignTokenAsync(new Account
		{
			Id = accountId,
		});
	}

	public Task<RefreshToken> RefreshTokenAsync(RefreshToken refreshToken)
	{
		return RefreshTokenAsync(refreshToken.Value);
	}

	public async Task<RefreshToken> RefreshTokenAsync(string refreshToken)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		var refreshToken2 = dbContext.RefreshTokens.Include(x => x.Account).First(x => x.Value == refreshToken);

		refreshToken2.Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(refreshToken2.Account!);

		await dbContext.SaveChangesAsync();

		return refreshToken2;
	}

	public async Task<RefreshToken> AssignOrRefreshTokenAsync(Account account)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		var refreshToken = dbContext.RefreshTokens.FirstOrDefault(x => x.AccountId == account.Id);

		if (refreshToken is not null)
		{
			refreshToken.Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(account);

			await dbContext.SaveChangesAsync();

			return refreshToken;
		}

		var entry = dbContext.RefreshTokens.Add(new RefreshToken
		{
			AccountId = account.Id,
			Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(account),
			Expires = DateTime.Now.AddMonths(12),
		});

		await dbContext.SaveChangesAsync();

		return entry.Entity;
	}
}