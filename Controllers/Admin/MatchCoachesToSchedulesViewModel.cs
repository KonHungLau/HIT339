using AnyoneForTennis.Models;

namespace AnyoneForTennis.Controllers.Admin
{
    internal class MatchCoachesToSchedulesViewModel
    {
        public List<Schedules> Schedules { get; set; }
        public List<Coach> Coaches { get; set; }
    }
}