using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsWebAPI.Hubs
{
    public class MessageHub : Hub
    {
        public Task Send(string message) 
        {
            return Clients.All.SendAsync("Send", message);
        }
    }
}
