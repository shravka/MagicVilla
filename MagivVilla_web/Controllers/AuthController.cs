using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagivVilla_web.Controllers
{
    public class AuthController :  Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginRequestDTO obj)
        {
            APIResponse result = await _authService.LoginAsync<APIResponse>(obj);
            if (result != null && result.IsSucess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(result.Result.ToString());


                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, loginResponseDTO.User.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Role, loginResponseDTO.User.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionString, loginResponseDTO.Token);
                return RedirectToAction("Index", "Home");
             }
            else
            {
                ModelState.AddModelError("CustomError", result.ErrorMessages.FirstOrDefault());
                return View(obj);
            }
            
        }


        [HttpGet]
        public IActionResult Register()
        {
           
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( RegistrationRequestDTO obj)
        {

            APIResponse result= await _authService.RegisterAsync<APIResponse>(obj);
            if(result!=null && result.IsSucess)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionString, "");

            return RedirectToAction("Index", "Home");
        }
    }
}
