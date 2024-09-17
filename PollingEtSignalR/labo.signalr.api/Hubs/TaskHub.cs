using labo.signalr.api.Controllers;
using labo.signalr.api.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework;

namespace labo.signalr.api.Hubs
{
    public class TaskHub : Hub
    {
        private UselessTasksController TasksController;
        private ApplicationDbContext _context;
        public TaskHub(ApplicationDbContext context)
        {
            _context = context;
            TasksController = new UselessTasksController(context);
        }

        static int nbUsers;

        public override async Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            nbUsers++;
            await Clients.All.SendAsync("UserCount", nbUsers);
            await Clients.Caller.SendAsync("TaskList",  _context.UselessTasks.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            base.OnDisconnectedAsync(exception);
            nbUsers--;
            await Clients.All.SendAsync("UserCount", nbUsers);
        }

        public async Task AddTask(string taskName)
        {
            await TasksController.Add(taskName);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }

        public async Task CompletedTask(int id)
        {
            await TasksController.Complete(id);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("TaskList", _context.UselessTasks.ToList());
        }
    }
}
