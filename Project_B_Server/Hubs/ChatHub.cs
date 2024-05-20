using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
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
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Client? disconnectedClient = await clientService.GetClientWithClientIdAsync(Context.ConnectionId);
        
        if (disconnectedClient is not null)
        {
            Console.WriteLine($"Client {disconnectedClient.ClientName} has disconnected.");
            await clientService.DeleteClientAsync(disconnectedClient.Id!); // Can't be null since it's set by MongoDB
            await Clients.All.SendAsync("ReceiveClientDisconnectedNotification", disconnectedClient.ClientName);
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task SendClientInfo(string user, float positionX, float positionY)
    {
        Console.WriteLine("Saving new client info: " + user + " with connection id: " + Context.ConnectionId + " at position (" + positionX + ", " + positionY + ")");
        await clientService.AddClientAsync(Context.ConnectionId, user, positionX, positionY);
        await Clients.All.SendAsync("ReceiveNewClientNotification", user, positionX, positionY);
    }
    
    public async Task SendClientsInfoToCaller()
    {
        var connectedClients = await clientService.GetClientsAsync();
        // send back only the client names
        await Clients.Caller.SendAsync("ReceiveClientsInfo", JsonSerializer.Serialize(connectedClients));
        Console.WriteLine("ConnectedClients.Count: " + connectedClients.Count);
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendPosition(string user, float x, float y, float rotationRadians, float directionX, float directionY)
    {
        Console.WriteLine($"[{DateTime.Now}] - {user} moved to ({x}, {y}) with rotation {rotationRadians} radians. Direction: ({directionX}, {directionY})");
        await Clients.All.SendAsync("ReceivePosition", user, x, y, rotationRadians, directionX, directionY);
        Task.Run(() => clientService.UpdateClientPositionAsync(user, x, y));
    }
}