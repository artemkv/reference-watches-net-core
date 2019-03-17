using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Watches.Controllers.Helpers;
using Watches.Exceptions;
using Watches.Mapper;
using Watches.Entities;
using Watches.Repositories;
using Watches.Models;

namespace Watches.Controllers
{
    [Route("api/watches")]
    [ApiController]
    public class WatchController : Controller
    {
        private IWatchRepository _watchRepository;
        private IApiConfiguration _config;

        public WatchController(IWatchRepository watchRepository, IApiConfiguration config)
        {
            _watchRepository = watchRepository;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<GetListResponse<WatchDto>>> GetWatchesAsync(
            [FromQuery] string title = null,
            [FromQuery] Gender? gender = null,
            [FromQuery] string brandTitle = null,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int? pageSize = null)
        {
            if (pageSize == null)
            {
                pageSize = _config.ApiDefaultPageSize;
            }

            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize((int)pageSize, _config.ApiPageSizeLimit);

            var watchesPage = await _watchRepository.GetWatchesAsync(title, gender, brandTitle, pageNumber, (int)pageSize);
            return new GetListResponse<WatchDto>
            {
                PageNumber = watchesPage.PageNumber,
                PageSize = watchesPage.PageSize,
                Total = watchesPage.Total,
                Count = watchesPage.Count,
                Results = watchesPage.Results.Select(watch => watch.ToWatchDto()).ToList()
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WatchDto>> GetWatchAsync(long id)
        {
            var watch = await _watchRepository.GetWatchAsync(id);
            if (watch == null)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return watch.ToWatchDto();
        }

        [HttpPost]
        public async Task<ActionResult<WatchDto>> CreateWatchAsync(WatchToPostDto watchDto)
        {
            var created = await _watchRepository.CreateWatchAsync(watchDto.ToWatch());
            return CreatedAtAction(nameof(GetWatchAsync), new { id = created.Id }, created.ToWatchDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchAsync(long id, WatchToPutDto watchDto)
        {
            if (id != watchDto.Id)
            {
                throw new BadRequestException($"Watch id {watchDto.Id} does not match the id in the route: {id}.", "id");
            }
            if (watchDto.Id == default(long))
            {
                throw new BadRequestException($"Watch id cannot be 0.", "id");
            }

            var updated = await _watchRepository.UpdateWatchAsync(watchDto.ToWatch());
            if (!updated)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchAsync(long id)
        {
            var removed = await _watchRepository.DeleteWatchAsync(id);
            if (!removed)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return NoContent();
        }
    }
}