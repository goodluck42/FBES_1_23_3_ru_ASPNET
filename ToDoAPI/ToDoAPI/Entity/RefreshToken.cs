namespace ToDoAPI.Entity;

public class RefreshToken
{
	public int Id { get; set; }

	public string Value { get; set; } = null!;
	
	public Account Account { get; set; } = null!;
	public int AccountId { get; set; }

	public DateTime Expires { get; set; }
}