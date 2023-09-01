using MagicVilla_web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MagivVilla_web.Models.ViewModel
{
    public class CreateVillaNumberVM
    {
        public CreateVillaNumberVM()
        {
            villaNumber= new VillaNumberCreateDTO();
        }
        
        public VillaNumberCreateDTO villaNumber { get; set; }

        [ValidateNever]  // we never validate when posting 
        public IEnumerable<SelectListItem> villaList { get; set; }

    }
}
