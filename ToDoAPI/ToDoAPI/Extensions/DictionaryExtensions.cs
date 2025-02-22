using System.Diagnostics.CodeAnalysis;

namespace ToDoAPI.Extensions;

public static class DictionaryExtensions
{
	public static bool TryGetValue<T>(this IDictionary<object, object?> source, object key,
		[NotNullWhen(true)] out T? typedResult)
	{
		typedResult = default;
	
		var hasKey = source.TryGetValue(key, out var result);
	
		if (hasKey)
		{
			typedResult = (T)result!;
		}
	
		return hasKey;
	}
}