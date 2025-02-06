namespace ToDoAPI.Services;

public readonly struct PaginationSegment(int offset, int count)
{
	public PaginationSegment() : this(0, 0)
	{
	}

	public int Offset { get; } = offset;
	public int Count { get; } = count;
}