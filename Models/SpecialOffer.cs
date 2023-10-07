using System;
using System.ComponentModel.DataAnnotations;

namespace AnyoneForTennis.Models
{
    public class SpecialOffer
    {
        [Key]
        public int SpecialOfferId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(10)]
        public string TargetAudience { get; set; } // "Members", "Coaches", or "Both"
    }
}
