using AutoMapper;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _Ilogger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public VillaController(ILogging logger ,IMapper mapper, IRepository repository)
        {
            _Ilogger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            return Ok(_mapper.Map<List<VillaDTO>>(await _repository.GetAsyncAll()));
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
        {
            if (id == 0) { return BadRequest(); }
            var result = await _repository.GetAsyncVilla(villa => villa.Id == id);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaCreateDTO>> CreateVilla([FromBody] VillaCreateDTO entity)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);

            //}
            var villa = await _repository.GetAsyncVilla(item => item.Name == entity.Name);
            if (villa!=null)
            {
                ModelState.AddModelError("CustomError", "Name already Exists");
            }

            if (entity == null) { return BadRequest(); }
            //Villa model = new()
            //{
            //    Name = entity.Name,
            //    Amenity = entity.Amenity,
            //    Details = entity.Details,
            //    ImageUrl = entity.ImageUrl,
            //    Occupancy = entity.Occupancy,
            //    Rate = entity.Rate,
            //    Sqft = entity.Sqft,
            //    CreatedDate = DateTime.Now

            //};
            var model = _mapper.Map<Villa>(entity);
            await _repository.CreateAsync(model);
            await _repository.SaveAsync();
            return CreatedAtRoute("GetVilla", new { Id = model.Id }, model);
           
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<VillaUpdateDTO>>UpdateVilla(VillaUpdateDTO entity)
        {
            var result = await _repository.GetAsyncVilla(villa => villa.Id == entity.Id,true);
            if(result == null) { return BadRequest(); }

            //Villa model = new()
            //{
            //    Id = entity.Id,
            //    Name = entity.Name,
            //    Amenity = entity.Amenity,
            //    Details = entity.Details,
            //    ImageUrl = entity.ImageUrl,
            //    Occupancy = entity.Occupancy,
            //    Rate = entity.Rate,
            //    Sqft = entity.Sqft,
            //    CreatedDate = DateTime.Now

            //};
            var model = _mapper.Map<Villa>(entity);
            await _repository.UpdateAsync(model);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Deletevilla(int id)
        {

            var result = await _repository.GetAsyncVilla(villa => villa.Id == id);

            if(result == null) { return BadRequest(); }
            await _repository.RemoveAsync(result);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch]
        public IEnumerable<VillaDTO> UpdateVillaPartially()
        {

            return new List<VillaDTO>()
            {
                new VillaDTO() {Id=1, Name="Luxury",Details=" "}
            };
        }


    }
}
