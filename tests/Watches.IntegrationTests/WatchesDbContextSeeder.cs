using System;
using System.Collections.Generic;
using System.Text;
using Watches.Data;
using Watches.Entities;

namespace Watches.IntegrationTests
{
    public class WatchesDbContextSeeder
    {
        public static readonly long ECO_DRIVE = 11;
        public static readonly long AUTOMATIC = 22;
        public static readonly long HAND_WIND = 33;
        public static readonly long QUARTZ = 44;

        public static readonly long CITIZEN = 111;
        public static readonly long OMEGA = 222;
        public static readonly long SEIKO = 333;
        public static readonly long ROLEX = 444;
        public static readonly long HAMILTON = 555;
        public static readonly long TIMEX = 666;

        public static readonly long CITIZEN_ECO_DRIVE = 1;
        public static readonly long SEIKO_5_AUTOMATIC = 2;
        public static readonly long ROLEX_SUBMARINER = 3;
        public static readonly long ROLEX_EXPLORER = 4;
        public static readonly long HAMILTON_KHAKI_FIELD = 5;
        public static readonly long TIMEX_CHRONOGRAPH = 6;

        public static void Seed(WatchesDbContext dbContext)
        {
            dbContext.Movements.AddRange(GetMovements());
            dbContext.Brands.AddRange(GetBrands());
            dbContext.Watches.AddRange(GetWatches());
            dbContext.SaveChanges();
        }

        private static List<Movement> GetMovements()
        {
            return new List<Movement>
            {
                new Movement {Id = ECO_DRIVE, Title = "Eco-Drive"},
                new Movement {Id = AUTOMATIC, Title = "Automatic"},
                new Movement {Id = HAND_WIND, Title = "Hand Wind"},
                new Movement {Id = QUARTZ, Title = "Quartz"}
            };
        }

        private static List<Brand> GetBrands()
        {
            return new List<Brand>
            {
                new Brand
                {
                    Id = CITIZEN,
                    Title = "Citizen",
                    YearFounded = 1918,
                    Description = "Citizen Watch is an electronics company primarily known for its watches"
                },
                new Brand
                {
                    Id = OMEGA,
                    Title = "Omega",
                    YearFounded = 1903,
                    Description = "Omega SA is a Swiss luxury watchmaker"
                },
                new Brand
                {
                    Id = SEIKO,
                    Title = "Seiko",
                    YearFounded = 1881,
                    Description = "Seiko Holdings Corporation is a Japanese holding company"
                },
                new Brand
                {
                    Id = ROLEX,
                    Title = "Rolex",
                    YearFounded = 1915,
                    Description = "Rolex SA is a Swiss luxury watch manufacturer based in Geneva"
                },
                new Brand
                {
                    Id = HAMILTON,
                    Title = "Hamilton",
                    YearFounded = 1892,
                    Description = "The Hamilton Watch Company is a Swiss manufacturer of wristwatches"
                },
                new Brand
                {
                    Id = TIMEX,
                    Title = "Timex",
                    YearFounded = 1854,
                    Description = "Timex is an American manufacturing company"
                }
            };
        }

        private static List<Watch> GetWatches()
        {
            return new List<Watch>
            {
                new Watch
                {
                    Id = CITIZEN_ECO_DRIVE,
                    Model = "AW2020-82L",
                    Title = "Titanium Eco-Drive",
                    Gender = Gender.Mens,
                    CaseSize = 41,
                    CaseMaterial = CaseMaterial.Titanium,
                    BrandId = CITIZEN,
                    MovementId = ECO_DRIVE
                },
                new Watch
                {
                    Id = SEIKO_5_AUTOMATIC,
                    Model = "SNZG15",
                    Title = "5 Sport Automatic",
                    Gender = Gender.Mens,
                    CaseSize = 41,
                    CaseMaterial = CaseMaterial.StainlessSteel,
                    BrandId = SEIKO,
                    MovementId = AUTOMATIC
                },
                new Watch
                {
                    Id = ROLEX_SUBMARINER,
                    Model = "114060",
                    Title = "Submariner",
                    Gender = Gender.Mens,
                    CaseSize = 40,
                    CaseMaterial = CaseMaterial.StainlessSteel,
                    BrandId = ROLEX,
                    MovementId = AUTOMATIC
                },
                new Watch
                {
                    Id = ROLEX_EXPLORER,
                    Model = "214270",
                    Title = "Explorer",
                    Gender = Gender.Mens,
                    CaseSize = 39,
                    CaseMaterial = CaseMaterial.StainlessSteel,
                    BrandId = ROLEX,
                    MovementId = AUTOMATIC
                },
                new Watch
                {
                    Id = HAMILTON_KHAKI_FIELD,
                    Model = "H68201943",
                    Title = "Khaki Field Blue Dial",
                    Gender = Gender.Mens,
                    CaseSize = 38,
                    CaseMaterial = CaseMaterial.StainlessSteel,
                    BrandId = HAMILTON,
                    MovementId = QUARTZ
                },
                new Watch
                {
                    Id = TIMEX_CHRONOGRAPH,
                    Model = "TW2R47500D7PF",
                    Title = "Allied Chronograph",
                    Gender = Gender.Mens,
                    CaseSize = 42,
                    CaseMaterial = CaseMaterial.Brass,
                    BrandId = TIMEX,
                    MovementId = QUARTZ
                }
            };
        }
    }
}
