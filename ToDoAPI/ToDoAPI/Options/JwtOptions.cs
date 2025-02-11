using Microsoft.Extensions.Options;

namespace ToDoAPI.Options;

public class JwtOptions
{
	// public JwtOptions Value { get; } = null!;

	public TimeSpan ExpiresIn { get; set; }
}