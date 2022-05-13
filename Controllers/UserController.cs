using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Shipstone.OpenBook.Data;
using Shipstone.OpenBook.Models;
using Shipstone.OpenBook.ViewModels;

namespace Shipstone.OpenBook.Controllers
{
    [Controller]
    public class UserController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<UserController> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly IUserStore<User> _UserStore;

        public UserController(
            ILogger<UserController> logger,
            Context context,
            UserManager<User> userManager,
            IUserStore<User> userStore
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._UserManager = userManager;
            this._UserStore = userStore;
        }

        [ActionName("Create")]
        [HttpGet]
        public IActionResult Create() => this.View();

        [ActionName("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(
            RegisterViewModel model,
            [FromQuery] String returnUrl
        )
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            User user = new User
            {
                CreatedUtc = DateTime.UtcNow
            };

            await this._UserStore.SetUserNameAsync(user, model.UserName, CancellationToken.None);
            IdentityResult result = await this._UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    this.ModelState.AddModelError(String.Empty, error.Description);
                }

                this._Logger.LogInformation($"Failed account creation at {DateTime.UtcNow}");
                return this.View(model);
            }

            this._Logger.LogInformation($"{user.UserName} created account at {DateTime.UtcNow}");
            return this.Redirect(returnUrl ?? "~/");
        }
    }
}
