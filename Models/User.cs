using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Shipstone.OpenBook.Models
{
    public class User : IdentityUser<int>
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Created at")]
        public DateTime Created => this.CreatedUtc.ToLocalTime();
        
        [DataType(DataType.DateTime)]
        [Display(Name = "Created at")]
        public DateTime CreatedUtc { get; set; }
    }
}
