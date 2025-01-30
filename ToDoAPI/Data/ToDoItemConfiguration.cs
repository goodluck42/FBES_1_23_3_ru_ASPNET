using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoAPI.Entity;
using ToDoAPI.Extensions;

namespace ToDoAPI.Data;

public sealed class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
	public void Configure(EntityTypeBuilder<ToDoItem> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Title).HasMaxLength(TitleMaxLength);
		builder.HasIndex(e => e.Title).IsUnique(false);
		builder.Property(e => e.Description).HasMaxLength(DescriptionMaxLength);
		builder.Ignore(e => e.IsCompleted);
		builder.Property(e => e.Version).IsConcurrencyToken();

		var faker = new Faker<ToDoItem>();
		int id = 0;

		var enumValues = Enum.GetValues<ToDoItemPriority>();
		
		faker.RuleFor(e => e.Id, _ => --id)
			.RuleFor(e => e.Title, f => f.Lorem.Sentence())
			.RuleFor(e => e.Description, f => f.Lorem.Paragraph())
			.RuleFor(e => e.Priority, _ => Random.Shared.Choice(enumValues));

		var fakeData = faker.Generate(1000);

		builder.HasData(fakeData);
	}

	public const int TitleMaxLength = 450;
	public const int DescriptionMaxLength = 640;
}