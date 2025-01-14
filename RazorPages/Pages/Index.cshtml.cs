using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Models;

namespace RazorPages.Pages;

public class IndexModel : PageModel
{
	public IndexModel()
	{
	}

	public IActionResult OnGet()
	{
		return Page();
	}

	public void OnPost(Game game)
	{
		if (game.Id != default
		    && !string.IsNullOrEmpty(game.Name)
		    && !string.IsNullOrEmpty(game.Genre))
		{
			GameList.S_Games.Add(game);
		}
	}
}