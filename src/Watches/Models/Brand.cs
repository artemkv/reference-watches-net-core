using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public long Id { get; set; }

        // TODO: when this timestamp is present, it is used to detect conflicts, but for now we don't allow clients to pass it
        // and we don't return it neither
//        [Timestamp]
//        public byte[] Ts { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int YearFounded { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public ICollection<Watch> Watches { get; set; }
    }
}
