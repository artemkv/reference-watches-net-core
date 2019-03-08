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

        public async Task<ResultsPage<Watch>> GetWatchesAsync(
            string title, Gender? gender, long? brandId, int pageNumber, int pageSize)
        {
            var query = _dbContext.Watches
                .Where(watch => String.IsNullOrEmpty(title) || watch.Title.Contains(title))
                .Where(watch => gender == null || watch.Gender == gender)
                .Where(watch => brandId == null || watch.BrandId == brandId)
                .Include(watch => watch.Brand);

            int total = await query.CountAsync();

            var results = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .OrderBy(watch => watch.Id)
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

        public async Task<Watch> CreateWatchAsync(Watch watch)
        {
            await _dbContext.Watches.AddAsync(watch);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(watch).Reference(x => x.Brand).LoadAsync();
            return watch;
        }

        public async Task<bool> UpdateWatchAsync(
            long id, string model, string title, Gender gender, int caseSize,
            CaseMaterial caseMaterial, long brandId, long movementId)
        {
            var watch = await _dbContext.Watches
                .FindAsync(id);

            if (watch == null)
            {
                return false;
            }

            watch.Title = title;
            watch.Model = model;
            watch.Title = title;
            watch.Gender = gender;
            watch.CaseSize = caseSize;
            watch.CaseMaterial = caseMaterial;
            watch.BrandId = brandId;
            watch.MovementId = movementId;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteWatchAsync(long id)
        {
            var watch = await _dbContext.Watches
                .FindAsync(id);

            if (watch == null)
            {
                return false;
            }

            _dbContext.Watches.Remove(watch);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
