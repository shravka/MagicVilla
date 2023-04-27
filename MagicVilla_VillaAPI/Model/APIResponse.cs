using System.Net;

namespace MagicVilla_VillaAPI.Model
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public Object Result { get; set; }
        public bool IsSucess {get; set;}
        public List<string> ErrorMessages { get; set; }

    }
}
