using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.Hubs
{
    public class PublicChatHub : Hub
    {
        private readonly UserManager<User> _UserManager;

        public PublicChatHub(UserManager<User> userManager) =>
            this._UserManager = userManager;

        public async Task SendMessageAsync(String message)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            await this.Clients.All.SendAsync(
                "ReceiveMessage",

                new
                {
                    Content = message,
                    SentUtc = DateTime.UtcNow,
                    UserName = this.Context.User.Identity.Name
                }
            );
        }
    }
}
