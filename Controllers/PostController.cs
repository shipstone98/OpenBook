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
    public class PostController : Controller
    {
        private readonly Context _Context;
        private readonly ILogger<PostController> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly IUserStore<User> _UserStore;

        public PostController(
            ILogger<PostController> logger,
            Context context,
            UserManager<User> userManager
        )
        {
            this._Context = context;
            this._Logger = logger;
            this._UserManager = userManager;
        }

        [ActionName("Create")]
        [Authorize]
        [HttpGet]
        public IActionResult Create() => this.View();

        [ActionName("Create")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Post model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            User user = await this._UserManager.GetUserAsync(this.User);
            StatusViewModel<int> id = await PostDao.CreateAsync(this._Context, user, model);
            this._Logger.LogInformation($"User {user.UserName} created post {id.ViewModel} at {DateTime.UtcNow}");
            return this.RedirectToAction("Details", new { id = id.ViewModel });
        }

        [ActionName("Details")]
        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int id)
        {
            StatusViewModel<Post> post = await PostDao.RetrieveAsync(this._Context, id);

            if (post.StatusCode == HttpStatusCode.NotFound)
            {
                return this.NotFound();
            }

            User user = await this._UserManager.GetUserAsync(this.User);

            return this.View(new PostViewModel
            {
                IsCreator = !(user is null) && user.Id == post.ViewModel.CreatorId,
                Post = post.ViewModel
            });
        }
    }
}
