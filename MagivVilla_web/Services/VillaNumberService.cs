using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Services.IServices;
using Newtonsoft.Json.Linq;

namespace MagivVilla_web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villaUrl;

        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration):base(clientFactory) 
        {
            _clientFactory = clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.POST,
                Data = dto,
                Url = villaUrl+ "/api/v1/VillaNumber",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id,string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.DELETE,
                Url = villaUrl + "/api/v1/villaNumber/" + id,
                Token = token
            }) ;
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = villaUrl + "/api/v1/villaNumber",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.GET,
                Url = villaUrl + "/api/v1/villaNumber/" + id,
                Token=token
            });
        
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APIType = SD.APIType.PUT,
                Data = dto,
                Url = villaUrl + "/api/v1/villaNumber/" + dto.VillaNo
            });
        }
    }
}
