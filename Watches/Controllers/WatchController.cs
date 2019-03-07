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
    [Route("api")]
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

        [HttpGet("watches")]
        public async Task<ActionResult<List<WatchDto>>> List()
        {
            var watches = await _watchService.GetWatches();
            return watches.Select(x => x.ToWatchDto()).ToList();
        }
    }
}