using System.Text.Json;
using ToDoAPI.Data;

namespace ToDoAPI.Results;

public class MyUnauthorizedResult<T>(T content) : IResult
{
	public async Task ExecuteAsync(HttpContext httpContext)
	{
		httpContext.Response.StatusCode = 401;
		httpContext.Response.ContentType = "application/json";
		
		await JsonSerializer.SerializeAsync(httpContext.Response.Body, content);
	}
}