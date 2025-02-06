using ToDoAPI.Exceptions;

namespace ToDoAPI.Extensions;

public static class ConfigurationExtensions
{
	public static string GetJwtAudience(this IConfiguration source)
	{
		return source[JwtAudience] ?? throw new ConfigurationSectionNotFoundException();
	}

	public static string GetJwtIssuer(this IConfiguration source)
	{
		return source[JwtIssuer] ?? throw new ConfigurationSectionNotFoundException();
	}

	public static string GetJwtSecret(this IConfiguration source)
	{
		return source[JwtSecret] ?? throw new ConfigurationSectionNotFoundException(); 
	}
	
	private const string JwtAudience = "JWTConfig:Audience";
	private const string JwtIssuer = "JWTConfig:Issuer";
	private const string JwtSecret = "JWTConfig:Secret";
}