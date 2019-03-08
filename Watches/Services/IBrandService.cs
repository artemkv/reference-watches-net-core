using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Services
{
    public interface IBrandService
    {
        Task<ResultsPage<Brand>> GetBrandsAsync(int pageNumber, int pageSize);
        Task<Brand> GetBrandAsync(long id);
        Task<bool> DeleteBrandAsync(long id);
    }
}
