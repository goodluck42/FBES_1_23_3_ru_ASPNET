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
			Expires = GetExpires(),
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

		refreshToken2.Expires = GetExpires();
		refreshToken2.Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(refreshToken2.Account!);

		await dbContext.SaveChangesAsync();

		return refreshToken2;
	}

	/// <summary>
	/// fsdfdsf
	/// </summary>
	/// <param name="account"></param>
	/// <returns></returns>
	public async Task<RefreshToken> AssignOrRefreshTokenAsync(Account account)
	{
		await using var dbContext = await dbContextFactory.CreateDbContextAsync();

		// RefreshToken(account, dbContext);

		var refreshToken = dbContext.RefreshTokens.FirstOrDefault(x => x.AccountId == account.Id);

		if (refreshToken is not null)
		{
			refreshToken.Expires = GetExpires();
			refreshToken.Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(account);

			await dbContext.SaveChangesAsync();

			return refreshToken;
		}

		var entry = dbContext.RefreshTokens.Add(new RefreshToken
		{
			AccountId = account.Id,
			Value = await refreshTokenGenerator.GenerateRefreshTokenAsync(account),
			Expires = GetExpires(),
		});

		await dbContext.SaveChangesAsync();

		return entry.Entity;
	}

	private static DateTime GetExpires() => DateTime.Now.AddMinutes(1);
}