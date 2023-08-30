using AutoMapper;
using MagicVilla_web.Models.DTO;

namespace MagicVilla_web
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaCreateDTO ,VillaDTO>().ReverseMap();
            CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();

            CreateMap<VillaNumberCreateDTO, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumberUpdateDTO, VillaNumberDTO>().ReverseMap();
        }
    }
}
