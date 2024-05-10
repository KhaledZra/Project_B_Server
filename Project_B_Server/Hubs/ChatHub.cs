using Microsoft.AspNetCore.SignalR;

namespace Project_B_Server.Hubs;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"[{DateTime.Now}] - Game client connected!");
        return base.OnConnectedAsync();
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendPosition(string user, float x, float y)
    {
        await Clients.All.SendAsync("ReceivePosition", user, x, y);
        Console.WriteLine($"[{DateTime.Now}] - {user} moved to ({x}, {y})");
    }
}