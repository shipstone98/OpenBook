using System.Diagnostics;
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
    public class HomeController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<HomeController> _Logger;
        private readonly UserManager<User> _UserManager;

        public HomeController(
            ILogger<HomeController> logger,
            Context context,
            UserManager<User> userManager
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._UserManager = userManager;
        }

        [ActionName("Index")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            User user = await this._UserManager.GetUserAsync(this.User);

            return this.View(new ProfileViewModel
            {
                IsCurrentUser = true,
                Posts = (await PostDao.RetrieveAllAsync(this._Context, user, true)).ViewModel,
                User = user
            });
        }

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
        });
    }
}
