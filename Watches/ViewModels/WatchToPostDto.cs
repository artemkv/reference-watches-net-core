using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Watches.Models;

namespace Watches.ViewModels
{
    public class WatchToPostDto
    {
        [Required, MaxLength(255)]
        public string Model { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int CaseSize { get; set; }

        [Required]
        public CaseMaterial CaseMaterial { get; set; }

        [Required]
        public long BrandId { get; set; }

        [Required]
        public long MovementId { get; set; }
    }
}
