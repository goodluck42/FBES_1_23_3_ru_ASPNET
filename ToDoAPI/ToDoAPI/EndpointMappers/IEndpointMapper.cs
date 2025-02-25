namespace ToDoAPI.EndpointMappers;

public interface IEndpointMapper
{
	static abstract void Map(IEndpointRouteBuilder endpoints);
}