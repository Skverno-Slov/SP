using Microsoft.AspNetCore.SignalR;

namespace LabWork.Server
{
    public class ChatHub : Hub
    {
        public async Task RetranslateMessage(string content, string user, string callbackName = "ReseiveMessage") => await Clients.All.SendAsync(callbackName, user, content);
    }
}
