using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly string secretKey;

        public UserRepository(ApplicationDBContext context,IConfiguration configuration)
        {
            _dbContext = context;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");
        }
        public bool IsUniqueUser(string userName)
        {
            var user = _dbContext.LocalUser.FirstOrDefault(x => x.UserName == userName);
            if(user!=null)return false;

            return true;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO request)
        {
            var user = _dbContext.LocalUser.FirstOrDefault(u => u.UserName.ToLower() == request.UserName.ToLower()
             && u.Password == request.Password);
            if (user == null)
                return null;

            //if user found generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            //claim - name of user , role of user etc
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()), //can use email as wll
                    new Claim(ClaimTypes.Role, user.Role)
                }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials= new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponseDTO()
            {
                token = tokenHandler.WriteToken(token),
                User = user
            };

        }

        public async  Task<LocalUser> Register(RegistrationRequestDTO registerRequest)
        {
            LocalUser user = new()
            {
                UserName = registerRequest.UserName,
                Role = registerRequest.Role,
                Password = registerRequest.Password,
                Name = registerRequest.Name

            };
            _dbContext.LocalUser.Add(user);
            await _dbContext.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
