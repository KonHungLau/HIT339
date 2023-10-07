using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AnyoneForTennis.Models
{
    public class CoachViewModel
    {
        public Coach Coach { get; set; }
        public List<Schedules> UpcomingSchedules { get; set; }
        public List<ApplicationUser> EnrolledApplicationUsers { get; set; }
    }
}
