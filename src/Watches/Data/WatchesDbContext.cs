using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.Data
{
    public class WatchesDbContext : DbContext
    {
        public WatchesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Movement> Movements { get; set; }
    }
}
