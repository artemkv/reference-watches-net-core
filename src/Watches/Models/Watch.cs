using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.Models
{
    [Table("Watch")]
    public class Watch
    {
        [Key]
        public long Id { get; set; }

        [Timestamp]
        public byte[] Ts { get; set; }

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
        public DateTime DateCreated { get; set; }

        [Required]
        public Brand Brand { get; set; }

        [Required]
        public long BrandId { get; set; }

        [Required]
        public Movement Movement { get; set; }

        [Required]
        public long MovementId { get; set; }
    }
}
