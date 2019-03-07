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
        public async Task<ActionResult<GetListResponse<BrandDto>>> GetBrandsAsync(
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 20)
        {
            if (pageNumber < 0)
            {
                return BadRequest($"Wrong value for page number: {pageNumber}. Page number is expected to be greater than 0.");
            }
            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest($"Wrong value for page size: {pageSize}. Page number is expected to be in 1-100 range.");
            }

            var brandsPage = await _brandService.GetBrandsAsync(pageNumber, pageSize);

            return new GetListResponse<BrandDto>
            {
                PageNumber = brandsPage.PageNumber,
                PageSize = brandsPage.PageSize,
                Total = brandsPage.Total,
                Count = brandsPage.Count,
                Results = brandsPage.Results.Select(brand => brand.ToBrandDto()).ToList()
            };
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