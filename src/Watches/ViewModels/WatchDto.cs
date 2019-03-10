using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Watches.Entities;

namespace Watches.ViewModels
{
    public class WatchDto
    {
        public long Id { get; set; }

        public string Model { get; set; }

        public string Title { get; set; }

        public Gender Gender { get; set; }

        public int CaseSize { get; set; }

        public CaseMaterial CaseMaterial { get; set; }

        public DateTime DateCreated { get; set; }

        public BrandDto Brand { get; set; }

        public long MovementId { get; set; }
    }
}
