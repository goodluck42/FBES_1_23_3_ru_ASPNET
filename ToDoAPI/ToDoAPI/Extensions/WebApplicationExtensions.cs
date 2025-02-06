using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Services;

namespace ToDoAPI.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication EnsureDatabaseCreated(this WebApplication source)
	{
		using var dbContext = source.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureCreated();

		return source;
	}

	public static WebApplication EnsureDatabaseDeleted(this WebApplication source)
	{
		using var dbContext = source.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureDeleted();

		return source;
	}

#if DEBUG
	public static WebApplication EnsureDatabaseDeletedAndCreated(this WebApplication source)
	{
		using var dbContext = source.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureDeleted();
		dbContext.Database.EnsureCreated();

		return source;
	}
#endif

	public static WebApplication MapEndpoints<TEndpointMapper>(this WebApplication source)
		where TEndpointMapper : IEndpointMapper
	{
		TEndpointMapper.Map(source);

		return source;
	}
}