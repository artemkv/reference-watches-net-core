using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Entities;
using Watches.Models;

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

        public static Brand ToBrand(this BrandToPostDto brand)
        {
            if (brand == null)
            {
                return null;
            }

            return new Brand
            {
                Title = brand.Title,
                YearFounded = brand.YearFounded,
                Description = brand.Description,
                DateCreated = DateTime.UtcNow
            };
        }

        public static Brand ToBrand(this BrandToPutDto brand)
        {
            if (brand == null)
            {
                return null;
            }

            return new Brand
            {
                Id = brand.Id,
                Title = brand.Title,
                YearFounded = brand.YearFounded,
                Description = brand.Description,
                DateCreated = DateTime.UtcNow
            };
        }
    }
}
