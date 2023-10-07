using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AnyoneForTennis.Controllers
{
    [Authorize(Roles = "Coach")]
    public class CoachController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CoachController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Profile()
        {
            var coachProfile = new Coach
            {
                FirstName = "John Doe",
                Biography = "Experienced tennis coach with over 10 years of coaching experience."
            };

            return View(coachProfile);
        }

        public async Task<IActionResult> UpcomingSchedules()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var schedules = await _context.Schedules
                .Where(s => s.CoachId == userId && s.Date > DateTime.UtcNow)
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.User)
                .ToListAsync();

            return View(schedules);
        }


        // 显示编辑资料表单
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            var coach = _context.Coaches.Find(user.Id);
            return View(coach);
        }

        // 处理编辑资料表单的提交
        [HttpPost]
        public async Task<IActionResult> EditProfile(Coach model)
        {

            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"{key}: {error.ErrorMessage}");
                }
            }

            var coach = _context.Coaches.Find(model.CoachId);

            if (coach == null)
            {
                // 没有找到对应的Coach，可以在这里添加一些调试输出或断点
                Debug.WriteLine($"No coach found with ID {model.CoachId}.");

                return View(model);
            }

            coach.FirstName = model.FirstName;
            coach.LastName = model.LastName;
            coach.Biography = model.Biography;
            _context.Update(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard"); 
        }


        public async Task<IActionResult> EnrolledApplicationUsers(int scheduleId)
        {
            // Query the database to retrieve enrolled members for the specified schedule
            var enrollments = await _context.Enrollments
                .Where(e => e.ScheduleId == scheduleId)
                .Include(e => e.User)
                .ToListAsync();

            Debug.WriteLine($"Enrollments count: {enrollments.Count}");

            return View(enrollments);
        }

        public async Task<IActionResult> Dashboard()
        {
            // 获取当前用户ID
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 获取教练信息
            var coach = await _context.Coaches.FindAsync(currentUserId);
            if (coach == null)
            {
                // 如果找不到教练信息，返回404错误
                return NotFound();
            }

            // 获取教练的即将进行的日程安排
            var upcomingSchedules = await _context.Schedules
                .Where(s => s.CoachId == currentUserId && s.Date >= DateTime.Now)
                .OrderBy(s => s.Date)
                .ToListAsync();

  
            var enrolledUserIds = upcomingSchedules
                .SelectMany(s => s.Enrollments.Select(e => e.UserId))
                .Distinct();

            var enrolledUsers = await _context.Users
                .Where(u => enrolledUserIds.Contains(u.Id))
                .ToListAsync();

         
            var model = new CoachViewModel
            {
                Coach = coach,
                UpcomingSchedules = upcomingSchedules,
                EnrolledApplicationUsers = enrolledUsers
            };

            return View(model);
        }



    }
}
