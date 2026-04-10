using Microsoft.AspNetCore.SignalR;

namespace RealTimeApp.Server
{
    public class ChatHub : Hub
    {
        public async Task Send(string username, string message/* string groupename*/)
        {
            await this.Clients.All.SendAsync("Recive", username,  message); //all -Groupe(grn)
        }

        //public async Task Enter(string groupeName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupeName)
        //}
    }
}
