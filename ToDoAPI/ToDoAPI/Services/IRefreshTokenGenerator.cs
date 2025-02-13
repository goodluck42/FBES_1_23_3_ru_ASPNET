using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IRefreshTokenGenerator
{
	Task<string> GenerateRefreshTokenAsync(Account account);
}