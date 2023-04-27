using AutoMapper;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            _response.Result = _mapper.Map<List<VillaDTO>>(await _repository.GetAsyncAll());
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            var villa = await _repository.GetAsyncVilla(item => item.Name == entity.Name);
            if (villa != null)
            {
                ModelState.AddModelError("CustomError", "Name already Exists");
                return BadRequest(ModelState);
            }

            if (entity == null) { return BadRequest(); }

            var model = _mapper.Map<Villa>(entity);
            await _repository.CreateAsync(model);
            _response.Result = _mapper.Map<VillaDTO>(model);
            _response.statusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVilla", new { Id = model.Id }, _response);

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<VillaUpdateDTO>> UpdateVilla(VillaUpdateDTO entity)
        {
            var result = await _repository.GetAsyncVilla(villa => villa.Id == entity.Id, true);
            if (result == null) { return BadRequest(); }

            var model = _mapper.Map<Villa>(entity);
            await _repository.UpdateAsync(model);
            _response.Result = _mapper.Map<VillaDTO>(model);
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> Deletevilla(int id)
        {
            var result = await _repository.GetAsyncVilla(villa => villa.Id == id);

            if (result == null) { return BadRequest(); }
            _response.Result = _mapper.Map<VillaDTO>(result);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        /********* patch to do  ******************/
    }
}
