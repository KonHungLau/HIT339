using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AnyoneForTennis.Models
{
    public class Schedules
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MaxParticipants { get; set; }

        [Required]
        public string CoachId { get; set; }
        public ApplicationUser Coach { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public Schedules()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}
