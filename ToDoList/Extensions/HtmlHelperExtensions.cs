using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Extensions;

public static class HtmlHelperExtensions
{
	public static IHtmlContent Submit(this IHtmlHelper source, object? htmlAttributes = null)
	{
		var builder = new StringBuilder();

		builder.Append("<input type=\"submit\"");

		if (htmlAttributes is not null)
		{
			WriteAttributes(builder, htmlAttributes);
		}

		builder.Append("/>");

		return new HtmlString(builder.ToString());
	}

	private static void WriteAttributes(StringBuilder builder, object htmlAttributes)
	{
		Type type = htmlAttributes.GetType();
		var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

		foreach (var property in properties)
		{
			var name = property.Name.ToLower();
			var value = property.GetValue(htmlAttributes);

			builder.Append($" {name}=\"{value}\"");
		}
	}
}