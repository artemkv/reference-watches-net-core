using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Services
{
    public interface IBrandService
    {
        Task<List<Brand>> GetBrandsAsync();
        Task<Brand> GetBrandAsync(long id);
    }
}
