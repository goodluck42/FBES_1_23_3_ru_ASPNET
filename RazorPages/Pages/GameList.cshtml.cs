using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Models;

namespace RazorPages.Pages;

public class GameList : PageModel
{
	public static List<Game> S_Games = [new Game(1, "Stalker 2", "survival"), new Game(2, "Rainbow Six Siege", "fps")];

	public IEnumerable<Game> Games { get; set; } = S_Games;

	[BindProperty(SupportsGet = true)] public string? Genre { get; set; }

	public IActionResult OnGet()
	{
		return Page();
	}

	public void OnPost()
	{
		
	}
}