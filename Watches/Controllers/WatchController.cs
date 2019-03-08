using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Watches.Controllers.Helpers;
using Watches.Exceptions;
using Watches.Mapper;
using Watches.Models;
using Watches.Services;
using Watches.ViewModels;

namespace Watches.Controllers
{
    [Route("api/watches")]
    [ApiController]
    public class WatchController : Controller
    {
        IWatchService _watchService;

        public WatchController(IWatchService watchService)
        {
            if (watchService == null)
            {
                throw new ArgumentNullException("watchService");
            }

            _watchService = watchService;
        }

        [HttpGet]
        public async Task<ActionResult<GetListResponse<WatchDto>>> GetWatchesAsync(
            [FromQuery] string title = "",
            [FromQuery] Gender? gender = null,
            [FromQuery] long? brandId = null,
            [FromQuery] int pageNumber = 0,
            [FromQuery] int pageSize = 20)
        {
            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize(pageSize);

            var watchesPage = await _watchService.GetWatchesAsync(title, gender, brandId, pageNumber, pageSize);
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
            var watch = await _watchService.GetWatchAsync(id);
            if (watch == null)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return watch.ToWatchDto();
        }

        [HttpPost]
        public async Task<ActionResult<WatchDto>> CreateWatchAsync(WatchToPostDto watch)
        {
            var created = await _watchService.CreateWatchAsync(watch.ToWatch());
            return CreatedAtAction(nameof(GetWatchAsync), new { id = created.Id }, created.ToWatchDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchAsync(long id)
        {
            var removed = await _watchService.DeleteWatchAsync(id);
            if (!removed)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return NoContent();
        }
    }
}