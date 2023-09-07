using AutoMapper;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    //route name is required this defines the path for the endpoint
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")] // this works as default is 1
    [ApiVersion("2.0")] // this fails as we need to pass some version to API.

    //multiple versions support needs to be mentioned on controller
    //this is not so right method as if controller name changes route will automatically change so better 
    // to use "api/Villa" harcode Name so no matter the name of controller route remains same

    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _Ilogger;
        private readonly IVillaRepository _repository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;

        public VillaController(ILogging logger, IMapper mapper, IVillaRepository repository)
        {
            _Ilogger = logger;
            _repository = repository;
            _mapper = mapper;
            _response = new APIResponse() { IsSucess = true };
        }

        [HttpGet]  // verb is also required else there will be error as it does not know what is the action
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            _response.Result = _mapper.Map<List<VillaDTO>>(await _repository.GetAsyncAll());
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}", Name = "GetVilla")] // {id:int} to ensure httpget calls are seperate from the previos one

        //to document return type we use ProducesResponseType
        //200 is hardcoded better to use below one which is more readable
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        //ActionResult for returning httpstats codes
        public async Task<ActionResult<APIResponse>> GetVillaById(int id)
        {

            if (id == 0)
            {
                return BadRequest(_response);
            }
            var result = await _repository.GetAsyncVilla(villa => villa.Id == id);
            if (result == null) { return NotFound(); }
            _response.Result = result;
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO entity)
        {
            try
            {
                var villa = await _repository.GetAsyncVilla(item => item.Name == entity.Name);
                if (villa != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Name already Exists");
                    return BadRequest(ModelState);
                }

                if (entity == null) { return BadRequest(); }

                var model = _mapper.Map<Villa>(entity);
                await _repository.CreateAsync(model);
                _response.Result = _mapper.Map<VillaDTO>(model);
                _response.statusCode = HttpStatusCode.Created;

                //createdAtRoute to add link where the resource was created
                return CreatedAtRoute("GetVilla", new { model.Id }, _response);

            }
            catch (Exception ex)
            {

                _response.IsSucess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }

            return _response;


        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                await _repository.UpdateAsync(_mapper.Map<Villa>(updateDTO));
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsSucess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSucess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
            //var result = await _repository.GetAsyncVilla(villa => villa.Id == entity.Id, true);
            //if (result == null) { return BadRequest(); }

            //var model = _mapper.Map<Villa>(entity);
            //await _repository.UpdateAsync(model);
            //_response.Result = _mapper.Map<VillaDTO>(model);
            //_response.statusCode = HttpStatusCode.OK;
            //return Ok(_response);
        }

        [Authorize(Roles = "Custom")]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> Deletevilla(int id)
        {
            if (id == 0) return BadRequest();
            var result = await _repository.GetAsyncVilla(villa => villa.Id == id);

            if (result == null) { return BadRequest(); }

            await _repository.RemoveAsync(result);
            _response.statusCode = HttpStatusCode.NoContent;
            _response.IsSucess = true;
            return Ok(_response);
        }

        /********* patch to do  ******************/
        [HttpPatch]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {

            if (patchDTO == null || id == 0) { return BadRequest(); }
            var result = await _repository.GetAsyncVilla(villa => villa.Id == id, true);

            if (result == null) { return BadRequest(); }
            var villDTO = _mapper.Map<VillaDTO>(result);

            patchDTO.ApplyTo(villDTO);
            return Ok(_response);

        }


    }
}
