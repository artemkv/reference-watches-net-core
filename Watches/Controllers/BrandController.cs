using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watches.Mapper;
using Watches.Services;
using Watches.ViewModels;

namespace Watches.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            if (brandService == null)
            {
                throw new ArgumentNullException("brandService");
            }

            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BrandDto>>> GetBrandsAsync()
        {
            var brands = await _brandService.GetBrandsAsync();
            return brands.Select(brand => brand.ToBrandDto()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandDto>> GetBrandAsync(string id)
        {
            long idParsed;
            if (!long.TryParse(id, out idParsed))
            {
                return BadRequest($"Incorrect value for id: '{id}'.");
            }

            var brand = await _brandService.GetBrandAsync(idParsed);
            if (brand == null)
            {
                return NotFound($"Brand with id {idParsed} cannot be found.");
            }
            return brand.ToBrandDto();
        }
    }
}