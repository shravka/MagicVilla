using AutoMapper;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Models;
using MagivVilla_web.Models.ViewModel;
using MagivVilla_web.Services;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MagivVilla_web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper,IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            CreateVillaNumberVM vm = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSucess)
            {
                vm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                });
              
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(CreateVillaNumberVM model)
        {
            if (ModelState.IsValid)
            {
               
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.villaNumber);

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else if(response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());                  
                }
                
            }

            var res = await _villaService.GetAllAsync<APIResponse>();
            if (res != null && res.IsSucess)
            {
                model.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(res.Result.ToString()).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }
            return View(model);
        }
        //public async Task<IActionResult> UpdateVillaNumber(int villaId)
        //{


        //    var response = await _villaNumberService.GetAsync<APIResponse>(villaId);

        //    if (response != null && response.IsSucess)
        //    {
        //        VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());

        //        return View(_mapper.Map<VillaNumberUpdateDTO>(model));
        //    }

        //    return NotFound();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVilla(VillaNumberUpdateDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaNumberService.UpdateAsync<APIResponse>(model);

        //        if (response != null && response.IsSucess)
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }
        //    }
        //    return View(model);
        //}

        //public async Task<IActionResult> DeleteVillaNumber(int villaId)
        //{
        //    var response = await _villaNumberService.GetAsync<APIResponse>(villaId);

        //    if (response != null && response.IsSucess)
        //    {
        //        VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
        //        return View(model);
        //    }

        //    return NotFound();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
        //{
        //    var response = await _villaNumberService.DeleteAsync<APIResponse>(villaDTO.Id);

        //    if (response != null && response.IsSucess)
        //    {
        //        return RedirectToAction(nameof(IndexVillaNumber));
        //    }

        //    return View(villaDTO);
        //}
    }
}