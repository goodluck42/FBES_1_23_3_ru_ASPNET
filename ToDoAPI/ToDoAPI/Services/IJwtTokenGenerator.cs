using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public interface IJwtTokenGenerator
{
	Task<string> GenerateJwtToken(Account account);
}