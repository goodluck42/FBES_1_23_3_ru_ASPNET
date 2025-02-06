using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoAPI.Entity;

namespace ToDoAPI.Data;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
	public void Configure(EntityTypeBuilder<Account> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Login).HasMaxLength(LoginMaxLength);
		builder.HasIndex(e => e.Login).IsUnique();
		builder.Property(e => e.Password).HasMaxLength(PasswordMaxLength);
	}

	public const int LoginMaxLength = 128;
	public const int PasswordMaxLength = 100;
}