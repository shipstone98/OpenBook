using System;
using System.ComponentModel.DataAnnotations;

namespace Shipstone.OpenBook.ViewModels
{
    public class RegisterViewModel : AccountViewModel
    {
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public String PasswordConfirmation { get; set; }
    }
}
