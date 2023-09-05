using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Services.IServices;

namespace MagivVilla_web.Services
{
    public class AuthService : BaseService ,IAuthService
    {
        public IHttpClientFactory httpClient { get; set; }
        public APIResponse res { get; set; }
        public string url { get; set; }

        public AuthService(IHttpClientFactory httpClientFactory,IConfiguration configuration): base(httpClientFactory)
        {
            this.res = new();
            this.httpClient = httpClientFactory;
            url = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = loginRequestDTO,
                Url = url + "/api/UsersAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO)
        {

            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = registrationRequestDTO,
                Url = url + "/api/UsersAuth/register"
            });
        }
    }
}
