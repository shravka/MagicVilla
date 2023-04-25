using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.VillaDataStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _Ilogger;
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;

        public VillaController(ILogging logger ,ApplicationDBContext dBContext, IMapper mapper)
        {
            _Ilogger = logger;
            _dbContext = dBContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_mapper.Map<List<VillaDTO>>(_dbContext.Villas));
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVillaById(int id)
        {
            if (id == 0) { return BadRequest(); }
            var result = _dbContext.Villas.FirstOrDefault(villa => villa.Id == id);
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaCreateDTO> CreateVilla([FromBody] VillaCreateDTO entity)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (_dbContext.Villas.Where(item => item.Name == entity.Name).Any())
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
            _dbContext.Villas.Add(model);
            _dbContext.SaveChanges();
            return CreatedAtRoute("GetVilla", new { Id = model.Id }, model);
           
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<VillaUpdateDTO> UpdateVilla(VillaUpdateDTO entity)
        {
            var result = _dbContext.Villas.AsNoTracking().FirstOrDefault(villa => villa.Id == entity.Id);
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
            _dbContext.Update(model);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult Deletevilla(int id)
        {

            var result = _dbContext.Villas.FirstOrDefault(villa => villa.Id == id);

            if(result == null) { return BadRequest(); }
            _dbContext.Villas.Remove(result); 
            _dbContext.SaveChanges();
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
