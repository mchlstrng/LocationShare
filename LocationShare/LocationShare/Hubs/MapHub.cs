using Microsoft.AspNetCore.SignalR;

namespace LocationShare.Hubs
{
    public class MapHub : Hub
    {
        // Method for clients to send their location to the server
        public async Task UpdateLocation(string roomName, string username, double latitude, double longitude)
        {
            // Broadcast the location to all clients in the specified room
            await Clients.Group(roomName).SendAsync("ReceiveLocationUpdate", username, latitude, longitude);
        }

        // Method for clients to join a specific room
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        // Method for clients to leave a specific room
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }
    }
}
