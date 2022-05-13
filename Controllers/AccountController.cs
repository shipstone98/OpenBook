using System;
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
    public class AccountController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<AccountController> _Logger;
        private readonly SignInManager<User> _SignInManager;
        private readonly UserManager<User> _UserManager;

        public AccountController(
            ILogger<AccountController> logger,
            Context context,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._SignInManager = signInManager;
            this._UserManager = userManager;
        }

        [ActionName("Login")]
        [HttpGet]
        public IActionResult Login() => this.View();

        [ActionName("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(
            LoginViewModel model,
            [FromQuery] String returnUrl
        )
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult result =
                await this._SignInManager.PasswordSignInAsync(
                    model.UserName,
                    model.Password,
                    model.IsPersistent,
                    true
                );

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    this._Logger.LogWarning($"{model.UserName} locked out at {DateTime.UtcNow}");
                    return this.RedirectToAction("LockedOut");
                }

                this._Logger.LogWarning($"{model.UserName} failed log in at {DateTime.UtcNow}");
                this.ModelState.AddModelError(String.Empty, "Either no account with the entered username exists, or the entered password is incorrect.");
                return this.View(model);
            }
            
            this._Logger.LogInformation($"{model.UserName} logged in at {DateTime.UtcNow}");
            return this.Redirect(returnUrl ?? "~/");
        }

        [ActionName("Logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync([FromQuery] String returnUrl)
        {
            User user = await this._UserManager.GetUserAsync(this.User);

            if (!(user is null))
            {
                this._Logger.LogInformation($"{user.UserName} logged out at {DateTime.UtcNow}");
                await this._SignInManager.SignOutAsync();
            }

            return this.Redirect(returnUrl ?? "~/");
        }
    }
}
