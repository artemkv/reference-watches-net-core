using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.ViewModels
{
    public class BrandToPutDto
    {
        [Required]
        public long Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int YearFounded { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }
    }
}
