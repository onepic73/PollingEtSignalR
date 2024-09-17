using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;

namespace labo.signalr.api.Hubs
{
    public class TaskHub : Hub
    {
        public static class UserHandler
        {
            public static HashSet<string> ConnectedIds = new HashSet<string>();
        }

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);
            await Clients.Caller.SendAsync("TaskList");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            await UserHandler.ConnectedIds.Remove();
            await Clients.All.SendAsync("UserCount", UserHandler.ConnectedIds.Count);

        }

        public async Task AddTask(string taskName)
        {

        }
    }
}
