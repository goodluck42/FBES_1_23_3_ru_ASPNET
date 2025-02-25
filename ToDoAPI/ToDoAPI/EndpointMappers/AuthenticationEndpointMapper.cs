global using ResultsApi = Microsoft.AspNetCore.Http.Results;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.Security;
using ToDoAPI.Services;


namespace ToDoAPI.EndpointMappers;

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
					return ResultsApi.BadRequest();
				}

				var account = mapper.Map<AccountDto, Account>(dto);

				await accountContext.AddAsync(account);

				return ResultsApi.Ok();
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
				var jwtToken = await jwtTokenGenerator.GenerateJwtTokenAsync(refreshToken.Account!);

				return ResultsApi.Json(new { JwtToken = jwtToken, RefreshToken = refreshToken.Value });
			}
			catch (InvalidOperationException)
			{
				return ResultsApi.Unauthorized();
			}
		});


		group.MapPost("/signin",
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
						return ResultsApi.BadRequest();
					}

					var account = await accountContext.GetAsync(dto.Login);

					if (account.Password != dto.Password)
					{
						// return ResultsApi.Extensions.ImTeapot();
						return ResultsApi.Extensions.Unauthorized("Ask your mom for password.");
					}

					var jwtToken = await jwtTokenGenerator.GenerateJwtTokenAsync(account);
					var refreshToken = await refreshTokenManager.AssignOrRefreshTokenAsync(account);

					return ResultsApi.Json(new
						{ JwtToken = jwtToken, RefreshToken = refreshToken.Value }); // stored in localStorage
				}
				catch
				{
					return ResultsApi.Unauthorized();
				}
			});

		group.MapGet("/claims", (ClaimsPrincipal claimsPrincipal) =>
		{
			return new
			{
				Claims = claimsPrincipal.Claims.Select(x => $"{x.Type} : '{x.Value}'")
			};
		});
		
		group.MapGet("/admin", () =>
		{
			return new
			{
				AdminData = "OK"
			};
		}).RequireAuthorization(Policies.AdminPolicy);

		group.MapGet("/user", () =>
		{
			return new
			{
				DefaultData = "OK"
			};
		}).RequireAuthorization(Policies.UserPolicy);
	}
}