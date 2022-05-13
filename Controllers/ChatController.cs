using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shipstone.OpenBook.Controllers
{
    [Controller]
    public class ChatController : Controller
    {
        [ActionName("Index")]
        [Authorize]
        [HttpGet]
        public IActionResult Index() => this.View();
    }
}
