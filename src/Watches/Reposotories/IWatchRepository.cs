using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Entities;

namespace Watches.Repositories
{
    public interface IWatchRepository
    {
        Task<ResultsPage<Watch>> GetWatchesAsync(
            string title, Gender? gender, string brandTitle, int pageNumber, int pageSize);
        Task<Watch> GetWatchAsync(long id);
        Task<Watch> CreateWatchAsync(Watch watch);
        Task<bool> UpdateWatchAsync(Watch watch);
        Task<bool> DeleteWatchAsync(long id);
    }
}
