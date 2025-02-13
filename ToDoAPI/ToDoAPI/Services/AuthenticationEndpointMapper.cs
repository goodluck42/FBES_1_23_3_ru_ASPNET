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
			async (
				[FromServices] IAccountContext accountContext,
				[FromServices] Mapper mapper,
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
		group.MapPost("/refresh", async (
			[FromServices] IJwtTokenGenerator jwtTokenGenerator,
			[FromServices] IRefreshTokenGenerator refreshTokenGenerator,
			[FromServices] IRefreshTokenManager refreshTokenManager,
			[FromBody] string rawRefreshToken) =>
		{
			try
			{
				var refreshToken = await refreshTokenManager.RefreshTokenAsync(rawRefreshToken);
				var jwtToken = jwtTokenGenerator.GenerateJwtToken(refreshToken.Account!);

				return Results.Json(new { JwtToken = jwtToken, RefreshToken = refreshToken.Value });
			}
			catch (InvalidOperationException)
			{
				return Results.Unauthorized();
			}
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
					var refreshToken = await refreshTokenManager.AssignOrRefreshTokenAsync(account);

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