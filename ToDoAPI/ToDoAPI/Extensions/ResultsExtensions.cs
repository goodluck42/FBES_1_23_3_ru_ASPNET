using ToDoAPI.Results;

namespace ToDoAPI.Extensions;

public static class ResultsExtensions
{
	public static IResult ImTeapot(this IResultExtensions source)
	{
		return ResultsApi.StatusCode(418);
	}

	public static IResult Unauthorized<T>(this IResultExtensions source, T content)
	{
		return new MyUnauthorizedResult<T>(content);
	}
}