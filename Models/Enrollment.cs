using System.ComponentModel.DataAnnotations;

namespace AnyoneForTennis.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 

        public int ScheduleId { get; set; }
        public Schedules Schedule { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
 }