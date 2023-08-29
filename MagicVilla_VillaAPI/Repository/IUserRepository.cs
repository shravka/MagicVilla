using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.Repository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        Task<LoginResponseDTO> Login(LoginRequestDTO request);

        Task<LocalUser> Register(RegistrationRequestDTO registerRequest);


    }
}
