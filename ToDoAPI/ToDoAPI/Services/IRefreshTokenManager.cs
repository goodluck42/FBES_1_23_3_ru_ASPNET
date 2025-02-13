using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IRefreshTokenManager
{
	Task<RefreshToken> AssignTokenAsync(Account account);
	Task<RefreshToken> AssignTokenAsync(int accountId);
	Task<RefreshToken> RefreshTokenAsync(RefreshToken refreshToken);
	Task<RefreshToken> RefreshTokenAsync(string refreshToken);
	
	Task<RefreshToken> AssignOrRefreshTokenAsync(Account account);
}