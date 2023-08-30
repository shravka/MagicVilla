using System.Net;

namespace MagicVilla_web.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public HttpStatusCode statusCode { get; set; }
        public Object Result { get; set; }
        public bool IsSucess {get; set;}
        public List<string> ErrorMessages { get; set; }

    }
}
