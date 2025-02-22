namespace ToDoAPI.Middlewares;

public interface IMiddleware
{
	// void Invoke(HttpContext context);
	Task InvokeAsync(HttpContext context);
}