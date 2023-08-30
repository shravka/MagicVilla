using AutoMapper;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Models;
using MagivVilla_web.Services;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MagivVilla_web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;

        public VillaNumberController(IVillaNumberService villaNumberService,IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
        }

        public  async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSucess)
            {
                list =JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVillaNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model);

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                 }
            }
            return View(model);
        }
        public async Task<IActionResult> UpdateVillaNumber(int villaId)
        {

            
                var response = await _villaNumberService.GetAsync<APIResponse>(villaId);

                if (response != null && response.IsSucess)
                {
                    VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());
                   
                    return View(_mapper.Map<VillaNumberUpdateDTO>(model));
                }
            
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaNumberUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model);

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSucess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
                return View(model);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
        {
           var response = await _villaNumberService.DeleteAsync<APIResponse>(villaDTO.Id);

           if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
           
           return View(villaDTO);
        }
    }
}