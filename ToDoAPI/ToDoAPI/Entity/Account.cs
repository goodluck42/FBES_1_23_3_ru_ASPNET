namespace ToDoAPI.Entity;

public class Account
{
	public int Id { get; set; }
	public string Login { get; set; } = null!;
	public string Password { get; set; } = null!;

	public ICollection<Role> Roles { get; set; } = null!;
}