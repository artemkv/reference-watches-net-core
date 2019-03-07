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
        public async Task<ActionResult<List<WatchDto>>> GetWatchesAsync()
        {
            var watches = await _watchService.GetWatchesAsync();
            return watches.Select(watch => watch.ToWatchDto()).ToList();
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