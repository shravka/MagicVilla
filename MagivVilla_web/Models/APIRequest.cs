using Microsoft.AspNetCore.Mvc;
using static MagicVilla_Utility.SD;

namespace MagicVilla_web.Models
{
    public class APIRequest
    {
        public APIType APIType { get; set; }
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}
