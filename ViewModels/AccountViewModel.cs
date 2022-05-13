using System;
using System.ComponentModel.DataAnnotations;

namespace Shipstone.OpenBook.ViewModels
{
    public abstract class AccountViewModel
    {
        [DataType(DataType.Password)]
        [Required]
        [StringLength(Internals._PasswordMaxLength, MinimumLength = Internals._PasswordMinLength)]
        public String Password { get; set; }

        [Display(Name = "User name")]
        [Required]
        [StringLength(Internals._UserNameMaxLength, MinimumLength = Internals._UserNameMinLength)]
        public String UserName { get; set; }
    }
}
