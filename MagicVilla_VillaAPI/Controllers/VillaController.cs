using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.VillaDataStore;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaData.villaList);
        }


        [HttpGet("{int:id}")]
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
        public void CreateVilla(VillaDTO entity)
        {
            VillaData.villaList.Add(entity);

        }

        //[HttpPut]
        //public IEnumerable<VillaDTO> UpdateVilla(VillaDTO entity)
        //{
        //    var result = VillaData.villaList.Where(villa => villa.Id == entity.Id).FirstOrDefault();

        //}

        [HttpDelete]
        public void Deletevilla(int id)
        {

            var result = VillaData.villaList.Where(villa => villa.Id == id).FirstOrDefault();
            if (result != null) { VillaData.villaList.Remove(result); }

        }

        //[HttpPatch]
        //public IEnumerable<VillaDTO> UpdateVillaPartially()
        //{

        //    return new List<VillaDTO>()
        //    {
        //        new VillaDTO() {Id=1, Name="Luxury",Details=" "}
        //    };
        //}


    }
}
