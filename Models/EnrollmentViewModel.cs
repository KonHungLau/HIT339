using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AnyoneForTennis.Models
{
    public class EnrollmentViewModel
    {
        // For storing the selected schedule ID
        public int SelectedScheduleId { get; set; }

        // To display all available schedules
        public List<SelectListItem> AvailableSchedules { get; set; }

        // Information about the coach that members wish to view
        public string CoachName { get; set; }
        public string CoachBio { get; set; } // Brief biography of the coach

        // Details about the selected schedule
        public DateTime ScheduleDate { get; set; }
        public string EventName { get; set; }
        public string Location { get; set; }
    }
}
