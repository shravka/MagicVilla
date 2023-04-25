using MagicVilla_VillaAPI.DTO;

namespace MagicVilla_VillaAPI.VillaDataStore
{
    public static class VillaData
    {
        public static List<VillaDTO> villaList { get; set; }
        static VillaData()
        {
            villaList=new List<VillaDTO>(){
                new VillaDTO() { Id = 1, Name = "Luxury", Details = " " },
                new VillaDTO() { Id = 2, Name = "Spacious", Details = " " }
            };
        }


    }
}
