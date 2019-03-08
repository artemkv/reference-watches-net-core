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

        public async Task<ResultsPage<Brand>> GetBrandsAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Brands;

            int total = await query.CountAsync();

            var results = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ResultsPage<Brand>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = total,
                Count = results.Count,
                Results = results
            };
        }

        public Task<Brand> GetBrandAsync(long id)
        {
            return _dbContext.Brands
                .FindAsync(id);
        }

        public async Task<Brand> CreateBrandAsync(Brand brand)
        {
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();
            return brand;
        }

        public async Task<bool> UpdateBrandAsync(long id, string title, int yearFounded, string description)
        {
            var brand = await _dbContext.Brands
                .FindAsync(id);

            if (brand == null)
            {
                return false;
            }

            brand.Title = title;
            brand.YearFounded = yearFounded;
            brand.Description = description;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBrandAsync(long id)
        {
            var brand = await _dbContext.Brands
                .FindAsync(id);

            if (brand == null)
            {
                return false;
            }

            _dbContext.Brands.Remove(brand);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
