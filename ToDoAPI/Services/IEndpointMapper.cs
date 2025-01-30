namespace ToDoAPI.Services;

public interface IEndpointMapper
{
	static abstract void Map(IEndpointRouteBuilder endpoints);
}