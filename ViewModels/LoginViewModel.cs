using System.ComponentModel.DataAnnotations;

namespace Shipstone.OpenBook.ViewModels
{
    public class LoginViewModel : AccountViewModel
    {
        [Display(Name = "Remember me?")]
        public bool IsPersistent { get; set; }
    }
}
