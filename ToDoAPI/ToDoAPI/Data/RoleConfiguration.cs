using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoAPI.Entity;

namespace ToDoAPI.Data;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Name).HasMaxLength(RoleMaxLength);
		builder.HasMany(e => e.Accounts).WithMany(e => e.Roles);
	}

	public const int RoleMaxLength = 42;
}