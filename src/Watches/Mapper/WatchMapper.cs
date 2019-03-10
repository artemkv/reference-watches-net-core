using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Entities;
using Watches.Models;

namespace Watches.Mapper
{
    public static class WatchMapper
    {
        public static WatchDto ToWatchDto(this Watch watch)
        {
            if (watch == null)
            {
                return null;
            }

            return new WatchDto
            {
                Id = watch.Id,
                Model = watch.Model,
                Title = watch.Title,
                Gender = watch.Gender,
                CaseSize = watch.CaseSize,
                CaseMaterial = watch.CaseMaterial,
                DateCreated = watch.DateCreated,
                Brand = watch.Brand.ToBrandDto(),
                MovementId = watch.MovementId
            };
        }

        public static Watch ToWatch(this WatchToPostDto watch)
        {
            if (watch == null)
            {
                return null;
            }

            return new Watch
            {
                Model = watch.Model,
                Title = watch.Title,
                Gender = watch.Gender,
                CaseSize = watch.CaseSize,
                CaseMaterial = watch.CaseMaterial,
                DateCreated = DateTime.UtcNow,
                BrandId = watch.BrandId,
                MovementId = watch.MovementId
            };
        }

        public static Watch ToWatch(this WatchToPutDto watch)
        {
            if (watch == null)
            {
                return null;
            }

            return new Watch
            {
                Id = watch.Id,
                Model = watch.Model,
                Title = watch.Title,
                Gender = watch.Gender,
                CaseSize = watch.CaseSize,
                CaseMaterial = watch.CaseMaterial,
                DateCreated = DateTime.UtcNow,
                BrandId = watch.BrandId,
                MovementId = watch.MovementId
            };
        }
    }
}
