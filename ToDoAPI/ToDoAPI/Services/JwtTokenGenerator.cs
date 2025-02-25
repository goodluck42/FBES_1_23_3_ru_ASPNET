using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.Options;

namespace ToDoAPI.Services;

public class JwtTokenGenerator(
	IConfiguration configuration,
	IAccountContext accountContext,
	IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
{
	public async Task<string> GenerateJwtTokenAsync(Account account)
	{
		var claims = new List<Claim> { new(ClaimsIdentity.DefaultNameClaimType, account.Login) };
		var roles = await accountContext.GetRolesAsync(account.Id);

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
		}

		var jwt = new JwtSecurityToken(configuration.GetJwtIssuer(), configuration.GetJwtAudience(), claims,
			null, DateTime.Now.Add(jwtOptions.Value.ExpiresIn),
			new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret())),
				SecurityAlgorithms.HmacSha256));
		var handler = new JwtSecurityTokenHandler();

		return handler.WriteToken(jwt);
	}
}