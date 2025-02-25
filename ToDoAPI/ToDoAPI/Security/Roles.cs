namespace ToDoAPI.Security;

public static class Roles
{
	static Roles()
	{
		Priorities = new Dictionary<string, int>()
		{
			{ Owner, 5000 },
			{ Admin, 4000 },
			{ Manager, 3000 },
			{ User, 2000 },
			{ Banned, 0 }
		};
	}

	public const string Owner = nameof(Owner);
	public const string Admin = nameof(Admin);
	public const string Manager = nameof(Manager);
	public const string User = nameof(User);
	public const string Banned = nameof(Banned);

	public static IReadOnlyDictionary<string, int> Priorities { get; }
}