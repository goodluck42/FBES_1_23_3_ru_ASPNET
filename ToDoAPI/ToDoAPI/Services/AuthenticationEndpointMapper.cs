using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
			async ([FromServices] IAccountContext accountContext, [FromServices] Mapper mapper,
				[FromBody] AccountDto? dto) =>
			{
				if (dto is null)
				{
					return Results.BadRequest();
				}

				var account = mapper.Map<AccountDto, Account>(dto);

				await accountContext.AddAsync(account);

				return Results.Ok();
			});

		group.MapPost("/login",
			async ([FromServices] IConfiguration configuration, [FromServices] IAccountContext accountContext,
				[FromBody] AccountDto dto) =>
			{
				try
				{
					if (dto?.Login is null)
					{
						return Results.BadRequest();
					}

					var account = await accountContext.GetAsync(dto.Login);

					if (account.Password != dto.Password)
					{
						return Results.Unauthorized();
					}

					var claims = new List<Claim> { new("Login", account.Login) };
					var roles = await accountContext.GetRolesAsync(account.Id);

					foreach (var role in roles)
					{
						claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name));
					}

					var jwt = new JwtSecurityToken(configuration.GetJwtIssuer(), configuration.GetJwtAudience(), claims,
						null, DateTime.Now.AddHours(1),
						new SigningCredentials(
							new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetJwtSecret())),
							SecurityAlgorithms.HmacSha256));

					var handler = new JwtSecurityTokenHandler();
					var token = handler.WriteToken(jwt);

					return Results.Json(new { Token = token });
				}
				catch
				{
					return Results.Unauthorized();
				}
			});

		group.MapGet("/login_info", [Authorize] async (ClaimsPrincipal principal) =>
		{
			var res = "";

			foreach (var claim in principal.Claims)
			{
				res += $"{claim.Type}: {claim.Value}\n";
			}

			return new
			{
				ClaimInfos = res
			};
		});

		group.MapGet("/admin", [Authorize(Roles = "admin")] async (ClaimsPrincipal principal) =>
		{
			var res = "";

			foreach (var claim in principal.Claims)
			{
				res += $"{claim.Type}: {claim.Value}\n";
			}

			return new
			{
				AdminData = "OK"
			};
		});

		group.MapGet("/user", [Authorize(Roles = "admin,user")] async (ClaimsPrincipal principal) =>
		{
			return new
			{
				DefaultData = "OK"
			};
		});
	}
}