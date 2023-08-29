using AutoMapper;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaCreateDTO ,Villa>().ReverseMap();
            CreateMap<VillaUpdateDTO, Villa>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumberCreateDTO, VillaNumber>().ReverseMap();
            CreateMap<VillaNumberUpdateDTO, VillaNumber>().ReverseMap();
        }
    }
}
