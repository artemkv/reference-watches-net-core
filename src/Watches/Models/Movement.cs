using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Watches.Models
{
    [Table("Movement")]
    public class Movement
    {
        [Key]
        public long Id { get; set; }

        [Timestamp]
        public byte[] Ts { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        public ICollection<Watch> Watches { get; set; }
    }
}
