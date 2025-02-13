using System.Security.Cryptography;
using System.Text;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
	public Task<string> GenerateRefreshTokenAsync(Account account)
	{
		var rawToken = $"{Guid.NewGuid()}_{account.Id}";
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
		
		return Task.FromResult(BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower());
	}
}