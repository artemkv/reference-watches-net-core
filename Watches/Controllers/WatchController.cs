using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Watches.Controllers.Helpers;
using Watches.Exceptions;
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
            PagingValidationHelper.ValidatePageNumber(pageNumber);
            PagingValidationHelper.ValidatePageSize(pageSize);

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
        public async Task<ActionResult<WatchDto>> GetWatchAsync(long id)
        {
            var watch = await _watchService.GetWatchAsync(id);
            if (watch == null)
            {
                return NotFound($"Watch with id {id} cannot be found.");
            }
            return watch.ToWatchDto();
        }
    }
}