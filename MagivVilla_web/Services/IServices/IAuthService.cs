using MagicVilla_web.Models.DTO;

namespace MagivVilla_web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO);

        Task<T> RegisterAsync<T>(RegistrationRequestDTO registrationRequestDTO);
    }
}
