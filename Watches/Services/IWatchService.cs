using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Services
{
    public interface IWatchService
    {
        Task<ResultsPage<Watch>> GetWatchesAsync(string title, int pageNumber, int pageSize);
        Task<Watch> GetWatchAsync(long id);
    }
}
