using System;

namespace Shipstone.OpenBook.ViewModels
{
    public class ErrorViewModel
    {
        public String RequestId { get; set; }

        public bool ShowRequestId => !String.IsNullOrEmpty(this.RequestId);
    }
}
