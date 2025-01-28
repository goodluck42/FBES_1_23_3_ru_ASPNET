using Microsoft.EntityFrameworkCore;
using ToDoList.Data;

namespace ToDoList.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication EnsureDatabaseCreated(this WebApplication app)
	{
		using var dbContext = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureCreated();

		return app;
	}

	public static WebApplication EnsureDatabaseDeleted(this WebApplication app)
	{
		using var dbContext = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureDeleted();

		return app;
	}

#if DEBUG
	public static WebApplication EnsureDatabaseDeletedAndCreated(this WebApplication app)
	{
		using var dbContext = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();

		dbContext.Database.EnsureDeleted();
		dbContext.Database.EnsureCreated();

		return app;
	}
#endif
}