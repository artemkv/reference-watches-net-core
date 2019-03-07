using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;
using Watches.ViewModels;

namespace Watches.Mapper
{
    public static class BrandMapper
    {
        public static BrandDto ToBrandDto(this Brand brand)
        {
            if (brand == null)
            {
                return null;
            }

            return new BrandDto
            {
                Id = brand.Id,
                Title = brand.Title,
                YearFounded = brand.YearFounded,
                Description = brand.Description,
                DateCreated = brand.DateCreated
            };
        }
    }
}
