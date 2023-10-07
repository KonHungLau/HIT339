using System.ComponentModel.DataAnnotations;

namespace AnyoneForTennis.Models
{
    public class Coach
    {
        [Key]
        public string CoachId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Biography { get; set; }

    }
}
