namespace ToDoAPI.Entity;

public class Account
{
	public int Id { get; set; }
	public string Login { get; set; } = null!;
	public string Password { get; set; } = null!;

	public RefreshToken RefreshToken { get; set; } = null!;
	public int? RefreshTokenId { get; set; }
	
	public ICollection<Role> Roles { get; set; } = null!;
}