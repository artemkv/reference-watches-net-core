using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.ViewModels
{
    public class BrandDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int YearFounded { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
