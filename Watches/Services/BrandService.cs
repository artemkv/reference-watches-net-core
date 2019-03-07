using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Data;
using Watches.Models;

namespace Watches.Services
{
    public class BrandService : IBrandService
    {
        WatchesDbContext _dbContext;

        public BrandService(WatchesDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            _dbContext = dbContext;
        }

        public Task<List<Brand>> GetBrandsAsync()
        {
            return _dbContext.Brands
                .ToListAsync();
        }

        public Task<Brand> GetBrandAsync(long id)
        {
            return _dbContext.Brands
                .FindAsync(id);
        }
    }
}
