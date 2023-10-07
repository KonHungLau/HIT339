using System.ComponentModel.DataAnnotations;

namespace AnyoneForTennis.Models
{
    public class CreateScheduleViewModel
    {
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Event Name is required")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Coach is required")]
        public string CoachId { get; set; }

        public List<Coach> Coaches { get; set; }
    }

}
