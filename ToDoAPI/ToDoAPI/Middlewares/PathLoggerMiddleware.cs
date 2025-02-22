using ToDoAPI.Extensions;

namespace ToDoAPI.Middlewares;

public class PathLoggerMiddleware : IMiddleware
{
	private readonly RequestDelegate _next;

	public PathLoggerMiddleware(RequestDelegate next)
	{
		_next = next;

		Console.WriteLine(nameof(PathLoggerMiddleware));
	}

	public async Task InvokeAsync(HttpContext context)
	{
		Console.WriteLine($"[{nameof(PathLoggerMiddleware)}] {DateTime.Now}: {context.Request.Path}\n");

		await _next(context);
		
		// if (context.Items.TryGetValue<bool>("MethodLoggerMiddlewareHandled", out var result))
		// {
		// 	if (result)
		// 	{
		// 		Console.WriteLine($"[{nameof(PathLoggerMiddleware)}] {DateTime.Now}: {context.Request.Path}\n");
		//
		// 		await _next(context);
		// 	}
		// 	else
		// 	{
		// 		context.Response.StatusCode = 500;
		// 	}
		// }
		// else
		// {
		// 	context.Response.StatusCode = 500;
		// }
	}
}