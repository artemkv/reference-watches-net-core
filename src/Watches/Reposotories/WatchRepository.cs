﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Data;
using Watches.Entities;

namespace Watches.Repositories
{
    public class WatchRepository : IWatchRepository
    {
        WatchesDbContext _dbContext;

        public WatchRepository(WatchesDbContext dbContext)
        {
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
            if (watch.Id != default(long))
            {
                throw new InvalidOperationException("Id is generated by the database, should be empty.");
            }
            await _dbContext.Watches.AddAsync(watch);
            await _dbContext.SaveChangesAsync();
            await _dbContext.Entry(watch).Reference(x => x.Brand).LoadAsync();
            return watch;
        }

        public async Task<bool> UpdateWatchAsync(Watch watch)
        {
            if (watch.Id == default(long))
            {
                throw new InvalidOperationException("Id of existing entity should be provided.");
            }
            _dbContext.Entry(watch).State = EntityState.Modified;
            _dbContext.Entry(watch).Property(x => x.DateCreated).IsModified = false;
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
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
