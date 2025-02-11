using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IRefreshTokenGenerator
{
	Task<string> GenerateRefreshToken(Account account);
}