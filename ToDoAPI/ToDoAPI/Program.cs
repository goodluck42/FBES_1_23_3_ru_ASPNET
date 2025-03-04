using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI;
using ToDoAPI.AuthorizationHandlers;
using ToDoAPI.Data;
using ToDoAPI.Dtos;
using ToDoAPI.EndpointMappers;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.HostedServices;
using ToDoAPI.Hubs;
using ToDoAPI.Middlewares;
using ToDoAPI.Options;
using ToDoAPI.Requirements;
using ToDoAPI.Security;
using ToDoAPI.Services;
using AuthenticationSchemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes;
using HttpVersion = System.Net.HttpVersion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(options => { options.ExpiresIn = TimeSpan.FromMinutes(1); });

builder.Services.AddTransient<IAuthorizationHandler, RoleHierarchyHandler>();
builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddSingleton(_ => MapperConfig.Init());
builder.Services.AddDbContextFactory<AppDbContext>();
builder.Services.AddScoped<IToDoContext, ToDoContextTest>();
builder.Services.AddScoped<IOffsetTodoItemPagination, ToDoContextTest>();
builder.Services.AddScoped<IToDoItemSorter, ToDoContextTest>();
builder.Services.AddScoped<IAccountContext, AccountContext>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddScoped<IRefreshTokenManager, RefreshTokenManager>();
builder.Services.AddControllers();

builder.Services.ConfigureHttpClientDefaults(b =>
{
	b.ConfigureHttpClient(client => { client.DefaultRequestVersion = HttpVersion.Version20; });
});
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidAudience = builder.Configuration.GetJwtAudience(),
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidIssuer = builder.Configuration.GetJwtIssuer(),
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetJwtSecret()))
	};
});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Policies.AdminPolicy, policy =>
	{
		// policy.RequireRole(Roles.Admin);

		policy.AddRequirements(new RoleHierarchyRequirement
		{
			Role = Roles.Admin
		});
	});

	options.AddPolicy(Policies.UserPolicy, policy =>
	{
		policy.AddRequirements(new RoleHierarchyRequirement
		{
			Role = Roles.User
		});

		//policy.RequireRole(Roles.Admin, Roles.User);

		// policy.RequireClaim();
	});
});
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<RefreshTokenCleanerBackgroundService>();
builder.Services.AddLogging(builder => { builder.AddConsole(); });

var app = builder.Build();

app.UseMiddleware<PathLoggerMiddleware>();
app.UseMiddleware<MethodLoggerMiddleware>();

app.UseHttpsRedirection();

app.UseCors(configure =>
{
	// configure.AllowAnyHeader()
	// 	.AllowAnyMethod()
	// 	.WithOrigins("http://localhost:5173")
	// 	.AllowCredentials();

	configure.SetIsOriginAllowed(origin => true) // Allows all origins
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowCredentials(); // Required for SignalR with authentication has context menu
});

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}


#if DEBUG
app.EnsureDatabaseCreated();
//app.EnsureDatabaseDeletedAndCreated();
#endif
app.MapEndpoints<ToDoEndpointMapper>();
app.MapEndpoints<AuthenticationEndpointMapper>();
app.MapControllers();

app.MapHub<ChatHub>("/chat");

app.Run();

// aspnet_project
// test123