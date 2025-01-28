using Microsoft.EntityFrameworkCore;
using ToDoList.Entity;

namespace ToDoList.Data;

public class AppDbContext(IConfiguration configuration) : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(configuration.GetConnectionString("SQLite"));
		// optionsBuilder.UseInMemoryDatabase(nameof(ToDoList));
		optionsBuilder.EnableSensitiveDataLogging();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//Console.WriteLine("<3");
		modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
	}

	public DbSet<ToDoItem> ToDoItems { get; set; }
}