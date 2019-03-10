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
        private IBrandService _brandService;
        private IApiConfiguration _config;

        public BrandController(IBrandService brandService, IApiConfiguration config)
        {
            if (brandService == null)
            {
                throw new ArgumentNullException("brandService");
            }
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            _brandService = brandService;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<GetListResponse<BrandDto>>> GetBrandsAsync(
            [FromQuery] int pageNumber = 0,
            [FromQuery] int? pageSize = null)
        {
            if (pageSize == null)
            {
                pageSize = _config.ApiDefaultPageSize;
            }

            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize((int)pageSize, _config.ApiPageSizeLimit);

            var brandsPage = await _brandService.GetBrandsAsync(pageNumber, (int)pageSize);

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

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrandAsync(BrandToPostDto brandDto)
        {
            var created = await _brandService.CreateBrandAsync(brandDto.ToBrand());
            return CreatedAtAction(nameof(GetBrandAsync), new { id = created.Id }, created.ToBrandDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandAsync(long id, BrandToPutDto brandDto)
        {
            if (id != brandDto.Id)
            {
                throw new BadRequestException($"Brand id {brandDto.Id} does not match the id in the route: {id}.", "id");
            }
            if (brandDto.Id == default(long))
            {
                throw new BadRequestException($"Brand id cannot be 0.", "id");
            }

            var updated = await _brandService.UpdateBrandAsync(brandDto.ToBrand());
            if (!updated)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return NoContent();
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