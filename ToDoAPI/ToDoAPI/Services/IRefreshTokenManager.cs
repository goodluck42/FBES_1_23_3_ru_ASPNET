using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IRefreshTokenManager
{
	Task AddOrUpdate(RefreshToken refreshToken);
	Task Add(RefreshToken refreshToken);
	Task Update(RefreshToken refreshToken);
	Task Remove(RefreshToken refreshToken);
	Task<RefreshToken> GetByAccountId(int accountId);
}