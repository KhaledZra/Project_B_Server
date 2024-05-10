using Microsoft.AspNetCore.SignalR;
using Project_B_Server_Domain;
using Project_B_Server.Services;

namespace Project_B_Server.Hubs;

public class ChatHub(ClientService clientService) : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("A new client has connected. Context.ConnectionId: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }
    
    // public override async Task OnDisconnectedAsync(Exception? exception)
    // {
    //     Client disconnectedClient = ConnectedClients.Find(c => c.ClientId == Context.ConnectionId);
    //     if (disconnectedClient is not null)
    //     {
    //         Console.WriteLine($"Client {disconnectedClient.ClientName} has disconnected.");
    //         ConnectedClients.Remove(disconnectedClient);
    //         await Clients.All.SendAsync("ReceiveClientDisconnectedNotification", disconnectedClient.ClientName);
    //     }
    //     await base.OnDisconnectedAsync(exception);
    // }
    
    public async Task SendClientInfo(string user)
    {
        Console.WriteLine("Saving new client info: " + user + " with connection id: " + Context.ConnectionId);
        await clientService.AddClientAsync(Context.ConnectionId, user);
        await Clients.All.SendAsync("ReceiveNewClientNotification", user);
        
        var connectedClients = await clientService.GetClientsAsync();
        await Clients.Caller.SendAsync("ReceiveClientsInfo", connectedClients);
        Console.WriteLine("ConnectedClients.Count: " + connectedClients.Count);
    }
    
    // public async Task SendClientsInfoToCaller()
    // {
    //     await Clients.Caller.SendAsync("ReceiveClientsInfo", ConnectedClients);
    // }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendPosition(string user, float x, float y, float rotationRadians)
    {
        Console.WriteLine($"[{DateTime.Now}] - {user} moved to ({x}, {y}) with rotation {rotationRadians} radians.");
        await Clients.All.SendAsync("ReceivePosition", user, x, y, rotationRadians);
    }
}