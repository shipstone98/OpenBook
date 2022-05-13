﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Shipstone.OpenBook.ViewModels;

namespace Shipstone.OpenBook.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _Logger;

        public HomeController(ILogger<HomeController> logger) =>
            this._Logger = logger;

        [Authorize]
        public IActionResult Index() => this.View();

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
        });
    }
}
