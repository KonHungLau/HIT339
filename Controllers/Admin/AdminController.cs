using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AnyoneForTennis.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            var schedules = await _context.Schedules.Include(s => s.Coach).ToListAsync();

            return View("AdminDashboard", schedules);
        }
        // get
        public IActionResult CreateSchedule()
        {
            // 获取所有教练以供选择
            var coaches = _context.Coaches.ToList();
            var viewModel = new CreateScheduleViewModel
            {
                Coaches = coaches
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateSchedule(CreateScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 创建新的计划并保存到数据库
                    var schedule = new Schedules
                    {
                        EventName = viewModel.EventName,
                        Date = viewModel.Date,
                        Location = viewModel.Location,
                        CoachId = viewModel.CoachId
                    };

                    _context.Schedules.Add(schedule);
                    _context.SaveChanges();

                    // 添加日志
                    Debug.WriteLine("Schedule created successfully!");

                    return RedirectToAction("AdminDashboard"); // 重定向到管理员仪表板或其他适当的页面
                }
                catch (Exception ex)
                {
                    // 捕获异常并进行处理，可以在控制台或日志中记录异常信息
                    Console.WriteLine($"Error creating schedule: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the schedule.");
                }

            }

            // 如果模型验证失败或发生异常，返回创建计划的视图并显示错误消息
            viewModel.Coaches = _context.Coaches.ToList();
            return View(viewModel);
        }
        public IActionResult ViewMembers()
        {
            var viewModel = new ViewMembersViewModel
            {
                Users = _context.Users.ToList()
            };
            return View(viewModel);
        }
        public async Task<IActionResult> AdminViewScheduleDetails()
        {
            // get all schedules from the database
            var viewModel = new EnrollmentViewModel
            {
                AvailableSchedules = await _context.Schedules
                                                   .Include(s => s.Coach) // Including the Coach (ApplicationUser in this case)
                                                   .Select(s => new SelectListItem
                                                   {
                                                       Value = s.ScheduleId.ToString(),
                                                       Text = $"{s.EventName} - {s.Coach.FirstName} {s.Coach.LastName}"
                                                   })
                                                   .ToListAsync()
            };

            return View(viewModel);
        }
    }
}