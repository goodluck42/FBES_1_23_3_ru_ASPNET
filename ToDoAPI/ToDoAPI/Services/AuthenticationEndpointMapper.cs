using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
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
			async (
				[FromServices] IAccountContext accountContext,
				[FromServices] IJwtTokenGenerator jwtTokenGenerator,
				[FromServices] IRefreshTokenGenerator refreshTokenGenerator,
				[FromServices] IRefreshTokenManager refreshTokenManager,
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

					var jwtToken = await jwtTokenGenerator.GenerateJwtToken(account);

					RefreshToken refreshToken;

					try
					{
						refreshToken = await refreshTokenManager.GetByAccountId(account.Id);
					}
					catch (InvalidOperationException)
					{
						refreshToken = new RefreshToken
						{
							AccountId = account.Id,
							Value = await refreshTokenGenerator.GenerateRefreshToken(account),
							Expires = DateTime.Now.AddMonths(12),
						};
					}

					await refreshTokenManager.AddOrUpdate(refreshToken);

					return Results.Json(new { JwtToken = jwtToken, RefreshToken = refreshToken.Value });
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