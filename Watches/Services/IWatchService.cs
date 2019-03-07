using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Services
{
    public interface IWatchService
    {
        Task<List<Watch>> GetWatchesAsync();
        Task<Watch> GetWatchAsync(long id);
    }
}
