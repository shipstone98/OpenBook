using System;
using System.Collections.Generic;

using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.ViewModels
{
    public class ProfileViewModel
    {
        public bool IsCurrentUser { get; set; }
        public bool IsFollowing { get; set; }
        public IReadOnlyList<Post> Posts { get; set; }
        public User User { get; set; }
    }
}
