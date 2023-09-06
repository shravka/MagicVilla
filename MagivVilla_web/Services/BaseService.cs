using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MagivVilla_web.Services
{
    public class BaseService :IBaseService
    {
        public APIResponse responseModel { get; set; }

        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.responseModel = new();
            this.httpClient = httpClientFactory; 
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
             
                if(apiRequest.Data!=null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),null, "application/json");
                }
                switch(apiRequest.APIType)
                {
                    case SD.APIType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.APIType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.APIType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if(ApiResponse != null && (ApiResponse.statusCode==HttpStatusCode.NotFound ||
                        ApiResponse.statusCode==HttpStatusCode.BadRequest)) 
                    {
                        ApiResponse.statusCode = HttpStatusCode.BadRequest;
                        ApiResponse.IsSucess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var resObject = JsonConvert.DeserializeObject<T>(res);
                        return resObject;
                    }
                   
                }
                catch (Exception)
                {

                    var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return APIResponse;
                }
                var result = JsonConvert.DeserializeObject<T>(apiContent);
                return result;

            }
            catch(Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSucess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}
