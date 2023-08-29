﻿using Azure;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UsersAuth")]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly APIResponse response;
        public UserController(IUserRepository userRepo)
        {
            userRepository = userRepo;
            response = new APIResponse()
            {
                IsSucess = true
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await userRepository.Login(model);
            if(loginResponse == null || string.IsNullOrEmpty(loginResponse.token))
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.IsSucess = false;
                response.ErrorMessages.Add("UserName or password is inCorrect");
                return BadRequest(response);

            }
            response.statusCode = HttpStatusCode.OK;
            response.Result = loginResponse;
            return Ok(response);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool isUserNameUnique = userRepository.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.IsSucess = false;
                response.ErrorMessages.Add("Username already exists");
                return BadRequest(response);
            }

            var user = await userRepository.Register(model);
            if (user == null)
            {
                response.statusCode = HttpStatusCode.BadRequest;
                response.IsSucess = false;
                response.ErrorMessages.Add("Error while registering");
                return BadRequest(response);
            }
            response.statusCode = HttpStatusCode.OK;
            return Ok(response);
        }
    }
}