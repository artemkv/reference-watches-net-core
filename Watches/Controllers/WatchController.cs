using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Watches.Mapper;
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

            var watchesPage = await _watchService.GetWatchesAsync(title, pageNumber, pageSize);
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
        public async Task<ActionResult<WatchDto>> GetWatchAsync(string id)
        {
            long idParsed;
            if (!long.TryParse(id, out idParsed)) {
                return BadRequest($"Incorrect value for id: '{id}'.");
            }

            var watch = await _watchService.GetWatchAsync(idParsed);
            if (watch == null)
            {
                return NotFound($"Watch with id {idParsed} cannot be found.");
            }
            return watch.ToWatchDto();
        }
    }
}