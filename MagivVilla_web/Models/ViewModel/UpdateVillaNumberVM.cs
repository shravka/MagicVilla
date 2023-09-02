using MagicVilla_web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MagivVilla_web.Models.ViewModel
{
    public class UpdateVillaNumberVM
    {
        public UpdateVillaNumberVM()
        {
            villaNumber= new VillaNumberUpdateDTO();
        }
        
        public VillaNumberUpdateDTO villaNumber { get; set; }

        [ValidateNever]  // we never validate when posting 
        public IEnumerable<SelectListItem> villaList { get; set; }

    }
}
