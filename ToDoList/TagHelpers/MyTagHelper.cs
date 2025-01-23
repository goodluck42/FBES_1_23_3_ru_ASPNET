using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ToDoList.TagHelpers;

public class MyTagHelper : TagHelper
{
	public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = "a";
		
		return base.ProcessAsync(context, output);
	}
}