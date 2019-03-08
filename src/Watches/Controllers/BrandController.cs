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
            [FromQuery] int pageSize = 20)
        {
            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize(pageSize, _config.ApiPageSizeLimit);

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

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrandAsync(BrandToPostDto brand)
        {
            var created = await _brandService.CreateBrandAsync(brand.ToBrand());
            return CreatedAtAction(nameof(GetBrandAsync), new { id = created.Id }, created.ToBrandDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrandAsync(long id, BrandToPutDto brand)
        {
            if (id != brand.Id)
            {
                throw new BadRequestException($"Brand id {brand.Id} does not match the id in the route: {id}.", "id");
            }

            var updated = await _brandService.UpdateBrandAsync(brand.Id, brand.Title, brand.YearFounded, brand.Description);
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