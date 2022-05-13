using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Shipstone.OpenBook.Data;
using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.Controllers
{
    [Controller]
    public class FollowingController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<FollowingController> _Logger;
        private readonly UserManager<User> _UserManager;

        public FollowingController(
            ILogger<FollowingController> logger,
            Context context,
            UserManager<User> userManager
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._UserManager = userManager;
        }

        [ActionName("Follow")]
        [Authorize]
        [HttpPost]
        [Route("[controller]/[action]/{userName}")]
        public async Task<IActionResult> FollowAsync(String userName, [FromQuery] String returnUrl)
        {
            User follower = await this._UserManager.GetUserAsync(this.User);
            User followee = await this._UserManager.FindByNameAsync(userName);

            if (followee is null)
            {
                return this.NotFound();
            }

            await FollowingDao.CreateAsync(
                this._Context,
                follower.Id,
                followee.Id
            );

            return this.Redirect(returnUrl ?? "~/");
        }

        [ActionName("Unfollow")]
        [Authorize]
        [HttpPost]
        [Route("[controller]/[action]/{userName}")]
        public async Task<IActionResult> UnfollowAsync(String userName, [FromQuery] String returnUrl)
        {
            User follower = await this._UserManager.GetUserAsync(this.User);
            User followee = await this._UserManager.FindByNameAsync(userName);

            if (followee is null)
            {
                return this.NotFound();
            }

            await FollowingDao.DeleteAsync(
                this._Context,
                follower.Id,
                followee.Id
            );

            return this.Redirect(returnUrl ?? "~/");
        }
    }
}
