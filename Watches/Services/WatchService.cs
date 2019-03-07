using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Data;
using Watches.Models;

namespace Watches.Services
{
    public class WatchService : IWatchService
    {
        WatchesDbContext _dbContext;

        public WatchService(WatchesDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
        }

        public async Task<ResultsPage<Watch>> GetWatchesAsync(string title, int pageNumber, int pageSize)
        {
            var query = _dbContext.Watches
                .Where(watch => watch.Title.Contains(title))
                .Include(watch => watch.Brand);

            int total = await query.CountAsync();

            var results = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ResultsPage<Watch>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = total,
                Count = results.Count,
                Results = results
            };
        }

        public async Task<Watch> GetWatchAsync(long id)
        {
            var watch = await _dbContext.Watches
                .FindAsync(id);

            if (watch == null)
            {
                return null;
            }

            await _dbContext.Entry(watch).Reference(x => x.Brand).LoadAsync();
            return watch;
        }
    }
}
