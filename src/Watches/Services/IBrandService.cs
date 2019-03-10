using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Entities;

namespace Watches.Services
{
    public interface IBrandService
    {
        Task<ResultsPage<Brand>> GetBrandsAsync(int pageNumber, int pageSize);
        Task<Brand> GetBrandAsync(long id);
        Task<Brand> CreateBrandAsync(Brand brand);
        Task<bool> UpdateBrandAsync(Brand brand);
        Task<bool> DeleteBrandAsync(long id);
    }
}
