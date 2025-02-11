using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoAPI.Entity;

namespace ToDoAPI.Data;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.Value).HasMaxLength(ValueMaxLength);
	}

	public const int ValueMaxLength = 32;
}