﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Services
{
    public interface IWatchService
    {
        Task<ResultsPage<Watch>> GetWatchesAsync(
            string title, Gender? gender, long? brandId, int pageNumber, int pageSize);
        Task<Watch> GetWatchAsync(long id);
        Task<Watch> CreateWatchAsync(Watch watch);
        Task<bool> UpdateWatchAsync(
            long id, string model, string title, Gender gender, int caseSize,
            CaseMaterial caseMaterial, long brandId, long movementId);
        Task<bool> DeleteWatchAsync(long id);
    }
}