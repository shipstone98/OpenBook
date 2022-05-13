using System.Net;

namespace Shipstone.OpenBook.ViewModels
{
    public class StatusViewModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T ViewModel { get; set; }
    }
}
