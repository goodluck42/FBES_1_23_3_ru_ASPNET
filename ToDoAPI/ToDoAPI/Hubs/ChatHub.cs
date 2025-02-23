using Microsoft.AspNetCore.SignalR;

namespace ToDoAPI.Hubs;

public class ChatHub : Hub
{
	public ChatHub()
	{
		Console.WriteLine("ChatHub");
	}

	public async Task SendMessage(ChatMessage message)
	{
		Console.WriteLine($"{message.Sender}: {message.Message}");
		await Clients.All.SendAsync("ReceiveMessage", message);
	}
	
	public override Task OnConnectedAsync()
	{
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}
}

public sealed record ChatMessage(string Sender, string Message);