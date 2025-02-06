using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;

namespace ToDoAPI.Services;

public class AuthenticationEndpointMapper : IEndpointMapper
{
	public static void Map(IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api");

		// 
		group.MapPost("/signup",
			async (IAccountContext accountContext, Mapper mapper, AccountDto? dto) =>
			{
				if (dto is null)
				{
					return Results.BadRequest();
				}

				var account = mapper.Map<AccountDto, Account>(dto);

				await accountContext.AddAsync(account);

				return Results.Ok();
			});

		group.MapGet("/login", async (IConfiguration configuration, IAccountContext accountContext, AccountDto? dto) =>
		{
			try
			{
				if (dto is { Login: not null })
				{
					return Results.BadRequest();
				}

				var account = await accountContext.GetAsync(dto.Login);
				var claims = new List<Claim> { new Claim("Login", account.Login) };
				var jwt = new JwtSecurityToken(configuration.GetJwtIssuer(), configuration.GetJwtAudience(), claims,
					null, DateTime.Now.AddMinutes(5),
					new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret())),
						SecurityAlgorithms.HmacSha256));

				var handler = new JwtSecurityTokenHandler();
				var token = handler.WriteToken(jwt);

				return Results.Json(new { Token = token });
			}
			catch
			{
				return Results.BadRequest();
			}
		});
	}
}