using MagicVilla_VillaAPI.Model;

namespace MagicVilla_VillaAPI.DTO
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
