using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watches.Controllers.Helpers;
using Watches.Exceptions;
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
            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize(pageSize);

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
        public async Task<ActionResult<BrandDto>> GetBrandAsync(long id)
        {
            var brand = await _brandService.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return brand.ToBrandDto();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrandAsync(long id)
        {
            var removed = await _brandService.DeleteBrandAsync(id);
            if (!removed)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return NoContent();
        }
    }
}