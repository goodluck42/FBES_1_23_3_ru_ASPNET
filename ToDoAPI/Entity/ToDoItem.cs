namespace ToDoAPI.Entity;

public class ToDoItem
{
	public int Id { get; set; }
	public string Title { get; set; } = null!;
	public string? Description { get; set; }
	public ToDoItemPriority Priority { get; set; }
	public DateTime? CompletionDateTime { get; set; }

	public bool IsCompleted => CompletionDateTime is not null;

	public Guid Version { get; set; }
}