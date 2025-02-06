using Microsoft.EntityFrameworkCore;
using ToDoAPI.Entity;

namespace ToDoAPI.Data;

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
	public DbSet<Role> Roles { get; set; }
	public DbSet<Account> Accounts { get; set; }
}