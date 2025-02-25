using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IJwtTokenGenerator
{
	Task<string> GenerateJwtTokenAsync(Account account);
}