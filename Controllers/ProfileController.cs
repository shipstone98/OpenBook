using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Shipstone.OpenBook.Data;
using Shipstone.OpenBook.Models;
using Shipstone.OpenBook.ViewModels;

namespace Shipstone.OpenBook.Controllers
{
    [Controller]
    public class ProfileController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<ProfileController> _Logger;
        private readonly UserManager<User> _UserManager;

        public ProfileController(
            ILogger<ProfileController> logger,
            Context context,
            UserManager<User> userManager
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._UserManager = userManager;
        }

        [ActionName("Index")]
        [HttpGet]
        [Route("[controller]/{userName?}")]
        public async Task<IActionResult> IndexAsync(String userName)
        {
            User user;
            bool isCurrentUser;

            if (userName is null)
            {
                user = await this._UserManager.GetUserAsync(this.User);

                if (user is null)
                {
                    return this.Unauthorized();
                }

                isCurrentUser = true;
            }

            else
            {
                User currentUser = await this._UserManager.GetUserAsync(this.User);
                user = await this._UserManager.FindByNameAsync(userName);

                if (user is null)
                {
                    return this.NotFound();
                }

                isCurrentUser = !(currentUser is null) && currentUser.Id == user.Id;
            }

            return this.View(new ProfileViewModel
            {
                IsCurrentUser = isCurrentUser,
                Posts = (await PostDao.RetrieveAllAsync(this._Context, user)).ViewModel,
                User = user
            });
        }
    }
}