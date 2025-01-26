using Bogus;
using Microsoft.EntityFrameworkCore;
using ToDoList.Entity;

namespace ToDoList.Data;

public class AppDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase(nameof(ToDoList));
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ToDoItem>(b =>
		{
			b.HasKey(e => e.Id);
			b.Property(e => e.Title).HasMaxLength(TitleMaxLength);
			b.HasIndex(e => e.Title).IsUnique(false);
			b.Property(e => e.Description).HasMaxLength(DescriptionMaxLength);
			b.Ignore(e => e.IsCompleted);
			b.Property(e => e.Version).IsConcurrencyToken();

			var faker = new Faker<ToDoItem>();
			int id = -1;
			faker.RuleFor(e => e.Id, --id)
				.RuleFor(e => e.Title, f => f.Lorem.Sentence())
				.RuleFor(e => e.Description, f => f.Lorem.Paragraph());

			b.HasData(faker.Generate(100));
		});
	}

	public DbSet<ToDoItem> ToDoItems { get; set; }

	public const int TitleMaxLength = 450;
	public const int DescriptionMaxLength = 640;
}