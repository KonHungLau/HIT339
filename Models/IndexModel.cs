using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection; // 导入此命名空间

namespace AnyoneForTennis.Pages
{
    public class IndexModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Schedules> Schedules { get; set; }
        public List<Coach> Coaches { get; set; }

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            Schedules = GetSampleSchedulesAsync().Result;
            Coaches = GetSampleCoachesAsync().Result;
        }

        private async Task<List<Schedules>> GetSampleSchedulesAsync()
        {
            var johnUser = await _userManager.FindByNameAsync("b@a.com"); 
            var alexUser = await _userManager.FindByNameAsync("a@a.com"); 


            return new List<Schedules>
            {
                new Schedules
                {
                    ScheduleId = 1,
                    EventName = "Tennis Lesson 1",
                    Date = DateTime.Now.AddDays(1),
                    Location = "Tennis Court 1",
                    MaxParticipants = 10,
                    CoachId = johnUser.Id
                },
                new Schedules
                {
                    ScheduleId = 2,
                    EventName = "Tennis Lesson 2",
                    Date = DateTime.Now.AddDays(2),
                    Location = "Tennis Court 2",
                    MaxParticipants = 12,
                    CoachId = alexUser.Id
                }
            };
        }

        private async Task<List<Coach>> GetSampleCoachesAsync()
        {
            var user1 = await _userManager.FindByNameAsync("user1");
            var user2 = await _userManager.FindByNameAsync("user2");

            return new List<Coach>
            {
                new Coach { CoachId = user1.Id, FirstName = "John Doe", Biography = "Experienced tennis coach with 10+ years of coaching." },
                new Coach { CoachId = user2.Id, FirstName = "Jane Smith", Biography = "Former professional tennis player turned coach." }
            };
        }
    }
}
