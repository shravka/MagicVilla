﻿using AutoMapper;
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
        public async Task<ActionResult<APIResponse>> GetVillaNumber()
        {
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(await _repository.GetAsyncAll());
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumberById(int id)
        {

            if (id == 0)
            {
                return BadRequest(_response);

            }
            var result = await _repository.GetAsyncVilla(villaNumber => villaNumber.VillaNo == id);
            if (result == null) { return NotFound(); }
            _response.Result = result;
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO entity)
        {

            var villaNumber = await _repository.GetAsyncVilla(item => item.VillaNo == entity.VillaNo);
            if (villaNumber != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists");
                return BadRequest(ModelState);
            }

            if (entity == null) { return BadRequest(); }

            var model = _mapper.Map<VillaNumber>(entity);
            await _repository.CreateAsync(model);
            _response.Result = _mapper.Map<VillaNumberDTO>(model);
            _response.statusCode = HttpStatusCode.Created;
            return CreatedAtRoute("GetVillaNumber", new { Id = model.VillaID }, _response);

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<VillaUpdateDTO>> UpdateVillaNumber(VillaNumberUpdateDTO entity)
        {
            var result = await _repository.GetAsyncVilla(villa => villa.VillaNo == entity.VillaNo, true);
            if (result == null) { return BadRequest(); }

            var model = _mapper.Map<VillaNumber>(entity);
            await _repository.UpdateAsync(model);
            _response.Result = _mapper.Map<VillaNumberDTO>(model);
            _response.statusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> DeletevillaNumber(int id)
        {
            var result = await _repository.GetAsyncVilla(villa => villa.VillaNo == id);

            if (result == null) { return BadRequest(); }
            _response.Result = _mapper.Map<VillaNumberDTO>(result);
            _response.statusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        /********* patch to do  ******************/
    }
}
