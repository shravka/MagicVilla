using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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

            return View();
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

        public IActionResult LogOut()
        {
            return View();
        }
    }
}
