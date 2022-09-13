using HC.Patient.Service.IServices;
using Microsoft.AspNetCore.SignalR;

namespace HC.Patient.Service.Services
{
    public class ChatHubService : Hub, IChatHubService
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }
    }
}
