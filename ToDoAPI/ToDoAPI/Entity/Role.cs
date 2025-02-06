namespace ToDoAPI.Entity;

public class Role
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	
	public ICollection<Account> Accounts { get; set; } = null!;
}