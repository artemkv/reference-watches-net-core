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

        public Task<List<Watch>> GetWatches()
        {
            return _dbContext.Watches
                .Include(watch => watch.Brand)
                .ToListAsync();
        }
    }
}
