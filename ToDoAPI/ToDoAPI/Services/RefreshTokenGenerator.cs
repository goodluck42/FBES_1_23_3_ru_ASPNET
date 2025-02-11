using System.Security.Cryptography;
using System.Text;
using ToDoAPI.Entity;

namespace ToDoAPI.Services;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
	public Task<string> GenerateRefreshToken(Account account)
	{
		var rawToken = $"{Guid.NewGuid()}_{account.Id}";
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
		
		// a44eff5d56ee4589921d3fa8c6adbf10
		
		return Task.FromResult(BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower());
	}
}