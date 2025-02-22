namespace ToDoAPI.Middlewares;

public class MethodLoggerMiddleware : IMiddleware
{
	private readonly RequestDelegate _next;

	public MethodLoggerMiddleware(RequestDelegate next)
	{
		_next = next;

		Console.WriteLine(nameof(MethodLoggerMiddleware));
	}

	public async Task InvokeAsync(HttpContext context)
	{
		// context.Items["MethodLoggerMiddlewareHandled"] = true;
		
		Console.WriteLine($"[{nameof(MethodLoggerMiddleware)}] {DateTime.Now}: {context.Request.Method}\n");

		await _next(context);
	}
}