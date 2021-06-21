using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Services.Hubs
{
    public class SignalServer : Hub
    {
        public Task AdminJoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public Task WarehouseManagerJoinGroup(string name)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, name);
        }
        public Task ShipperJoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
