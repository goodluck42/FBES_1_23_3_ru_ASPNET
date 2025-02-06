namespace ToDoAPI.Extensions;

public static class RandomExtensions
{
	public static T Choice<T>(this Random random, IEnumerable<T> values)
	{
		var vals = values.ToList();

		return vals.ElementAt(random.Next(0, vals.Count));
	}
}