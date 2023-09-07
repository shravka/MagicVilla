using AutoMapper;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Migrations;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogging _Ilogger;
        private readonly IVillaNumberRepository _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public VillaNumberController(ILogging logger, IMapper mapper, IVillaNumberRepository repository)
        {
            _Ilogger = logger;
            _repository = repository;
            _mapper = mapper;
            _response = new APIResponse() { IsSucess = true };
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [MapToApiVersion("2")]
        public string GetVillaNumberV2()
        {
            return "abc" + " defgjg";
        }

      
    }
}
