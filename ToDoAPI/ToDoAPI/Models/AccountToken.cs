namespace ToDoAPI.Models;

public class AccountToken
{
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }

	public bool IsValid()
	{
		return !string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(RefreshToken);
	}
}

