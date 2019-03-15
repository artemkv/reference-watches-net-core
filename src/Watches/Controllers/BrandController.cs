using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watches.Controllers.Helpers;
using Watches.Exceptions;
using Watches.Mapper;
using Watches.Repositories;
using Watches.Models;

namespace Watches.Controllers
{
    [Route("api/brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private IBrandRepository _brandRepository;
        private IApiConfiguration _config;

        public BrandController(IBrandRepository brandRepository, IApiConfiguration config)
        {
            _brandRepository = brandRepository;
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

            var brandsPage = await _brandRepository.GetBrandsAsync(pageNumber, (int)pageSize);

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
            var brand = await _brandRepository.GetBrandAsync(id);
            if (brand == null)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return brand.ToBrandDto();
        }

        [HttpPost]
        public async Task<ActionResult<BrandDto>> CreateBrandAsync(BrandToPostDto brandDto)
        {
            var created = await _brandRepository.CreateBrandAsync(brandDto.ToBrand());
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

            var updated = await _brandRepository.UpdateBrandAsync(brandDto.ToBrand());
            if (!updated)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrandAsync(long id)
        {
            var removed = await _brandRepository.DeleteBrandAsync(id);
            if (!removed)
            {
                return NotFound($"Brand with id {id} cannot be found.");
            }
            return NoContent();
        }
    }
}