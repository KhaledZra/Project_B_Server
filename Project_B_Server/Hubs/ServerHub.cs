using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Project_B_Server_Domain;
using Project_B_Server.Services;
using Serilog;

namespace Project_B_Server.Hubs;

public class ServerHub(ClientService clientService) : Hub
{
    // Log the connection of a new client.
    public override Task OnConnectedAsync()
    {
        Log.Information("A new client has connected. Context.ConnectionId: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }
    
    // Delete the disconnected client from the db and tell the other clients about the disconnection.
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Client? disconnectedClient = await clientService.GetClientWithClientIdAsync(Context.ConnectionId);
        
        if (disconnectedClient is not null)
        {
            Log.Information("Client {ClientName} has disconnected", disconnectedClient.ClientName);
            await clientService.DeleteClientAsync(disconnectedClient.Id!); // Can't be null since it's set by MongoDB
            await Clients.All.SendAsync("ReceiveClientDisconnectedNotification", disconnectedClient.ClientName);
        }
        await base.OnDisconnectedAsync(exception);
    }
    
    // Save the new connected client and tell the others about the new client.
    public async Task SendClientInfo(string user, string clientPlayerSpriteName, string clientNickName, float positionX, float positionY)
    {
        Log.Information("Saving new client info: {User} with connection id: {ConnectionId} at position ({X}, {Y})",
            user, Context.ConnectionId, positionX, positionY);
        await clientService.AddClientAsync(Context.ConnectionId, user, clientPlayerSpriteName, clientNickName, positionX, positionY);
        await Clients.All.SendAsync("ReceiveNewClientNotification", user, clientPlayerSpriteName, clientNickName, positionX, positionY);
    }
    
    // Sends information to the client caller about all the connected clients to help them sync the game.
    public async Task SendClientsInfoToCaller()
    {
        var connectedClients = await clientService.GetClientsAsync();
        await Clients.Caller.SendAsync("ReceiveClientsInfo", JsonSerializer.Serialize(connectedClients));
        Log.Information("ConnectedClients.Count: {Count}", connectedClients.Count);
    }
    
    // Todo: needs to be removed
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Sends the position of the player to all the clients when they move and updates the db with the position to help keep last known coordinates stored.
    public async Task SendPosition(string user, float x, float y, float rotationRadians, float directionX, float directionY)
    {
        // Log.Information("[{Now}] - {User} moved to ({X}, {Y}) with rotation {RotationRadians} radians. Direction: ({DirectionX}, {DirectionY})",
        //     DateTime.Now, user, x, y, rotationRadians, directionX, directionY);
        await Clients.All.SendAsync("ReceivePosition", user, x, y, rotationRadians, directionX, directionY);
        Task.Run(() => clientService.UpdateClientPositionAsync(user, x, y));
    }
}