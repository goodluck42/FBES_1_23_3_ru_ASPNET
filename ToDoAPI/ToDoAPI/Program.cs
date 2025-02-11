using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI;
using ToDoAPI.Data;
using ToDoAPI.Dtos;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;
using ToDoAPI.Options;
using ToDoAPI.Services;
using AuthenticationSchemes = Microsoft.AspNetCore.Server.HttpSys.AuthenticationSchemes;
using HttpVersion = System.Net.HttpVersion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(options => { options.ExpiresIn = TimeSpan.FromMinutes(30); });

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
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}

app.UseCors(configure => { configure.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
app.UseHttpsRedirection();

#if DEBUG
app.EnsureDatabaseCreated();
//app.EnsureDatabaseDeletedAndCreated();
#endif
app.MapEndpoints<ToDoEndpointMapper>();
app.MapEndpoints<AuthenticationEndpointMapper>();

app.Run();