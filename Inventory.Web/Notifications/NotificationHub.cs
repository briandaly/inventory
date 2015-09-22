using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Inventory
{
    public class NotificationHub : Hub
    {
        public void Send(string name, string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.broadcastMessage(name, message);
        }
    }
}