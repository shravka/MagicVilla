using AutoMapper;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Models;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace MagivVilla_web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaService _villaService;

        public VillaController(IVillaService villaService,IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        public  async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSucess)
            {
                list =JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles="admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(model);

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                 }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {

            
                var response = await _villaService.GetAsync<APIResponse>(villaId);

                if (response != null && response.IsSucess)
                {
                    VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
                   
                    return View(_mapper.Map<VillaUpdateDTO>(model));
                }
            
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(model);

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);

            if (response != null && response.IsSucess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(response.Result.ToString());
                return View(model);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(VillaDTO villaDTO)
        {
           var response = await _villaService.DeleteAsync<APIResponse>(villaDTO.Id);

           if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
           
           return View(villaDTO);
        }
    }
}