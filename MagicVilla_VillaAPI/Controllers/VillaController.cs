using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.VillaDataStore;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _Ilogger;
        private readonly ApplicationDBContext _dbContext;

        VillaController(ILogging logger ,ApplicationDBContext dBContext)
        {
            _Ilogger = logger;
            _dbContext = dBContext;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaData.villaList);
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVillaById(int id)
        {
            if (id == 0) { return BadRequest(); }
            var result = VillaData.villaList.Where(villa => villa.Id == id).FirstOrDefault();
            if (result == null) { return NotFound(); }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO entity)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaData.villaList.Where(item => item.Name == entity.Name).Any())
            {
                ModelState.AddModelError("CustomError", "Name already Exists");
            }

            if (entity == null) { return BadRequest(); }

            VillaData.villaList.Add(entity);
            Console.WriteLine(VillaData.villaList.Count);
            return CreatedAtRoute("GetVilla", new VillaDTO { Id = entity.Id }, entity);
            //return Ok(entity);
        }

        [HttpPut]
        public ActionResult<VillaDTO> UpdateVilla(VillaDTO entity)
        {
            var result = VillaData.villaList.Where(villa => villa.Id == entity.Id).FirstOrDefault();
            result.Name = entity.Name;
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public ActionResult Deletevilla(int id)
        {

            var result = VillaData.villaList.Where(villa => villa.Id == id).FirstOrDefault();
            if (result != null) { VillaData.villaList.Remove(result); }
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
