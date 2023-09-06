using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagivVilla_web.Models;
using MagivVilla_web.Models.ViewModel;
using MagivVilla_web.Services;
using MagivVilla_web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
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
            var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));

            if (response != null && response.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            CreateVillaNumberVM vm = new();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber(CreateVillaNumberVM model)
        {
            if (ModelState.IsValid)
            {
               
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.villaNumber, HttpContext.Session.GetString(SD.SessionString));

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else if(response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages",response.ErrorMessages.FirstOrDefault());                  
                }
                
            }

            var res = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {

            UpdateVillaNumberVM vm = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionString));
            if (response != null && response.IsSucess)
            {
                var model = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());
                vm.villaNumber=_mapper.Map<VillaNumberUpdateDTO>(model);
            }

            response =await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(UpdateVillaNumberVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.villaNumber, HttpContext.Session.GetString(SD.SessionString));

                if (response != null && response.IsSucess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            var res = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {

            DeleteViewModelVM vm =new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionString));
            if (response != null && response.IsSucess)
            {
                vm.villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(response.Result.ToString());
          
            }
            response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionString));
            vm.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(response.Result.ToString()).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            });

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(DeleteViewModelVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.villaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionString));

            if (response != null && response.IsSucess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}