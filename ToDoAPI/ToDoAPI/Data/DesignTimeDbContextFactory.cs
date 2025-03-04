using Microsoft.EntityFrameworkCore.Design;

namespace ToDoAPI.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var builder = new ConfigurationBuilder();

		builder.AddJsonFile("appsettings.json");

		return new AppDbContext(builder.Build());
	}
}