﻿

using MagivVilla_web.Models.DTO;

namespace MagicVilla_web.Models.DTO
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
