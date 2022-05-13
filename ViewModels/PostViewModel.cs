using System;

using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.ViewModels
{
    public class PostViewModel
    {
        public bool IsCreator { get; set; }
        public Post Post { get; set; }
    }
}
