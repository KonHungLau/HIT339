using AnyoneForTennis.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System;
using AnyoneForTennis.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AnyoneForTennis.Controllers.Membership
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var model = new EnrollmentViewModel
            {
                AvailableSchedules = GetAvailableSchedules()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EnrollmentViewModel model, bool redirectToCreate = false)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var existingEnrollment = _context.Enrollments.FirstOrDefault(e => e.UserId == userId && e.ScheduleId == model.SelectedScheduleId);

                var schedule = _context.Schedules.FirstOrDefault(s => s.ScheduleId == model.SelectedScheduleId);
                if (schedule == null)
                {
                    return NotFound();
                }

                if (existingEnrollment != null)
                {
                    TempData["ErrorMessage"] = "You are already enrolled in this course.";
                }
                else if (DateTime.Now > schedule.Date)
                {
                    TempData["ErrorMessage"] = "Course enrollment has ended.";
                }
                else
                {
                    var enrollment = new Enrollment
                    {
                        ScheduleId = model.SelectedScheduleId,
                        UserId = userId,
                        EnrollmentDate = DateTime.Now
                    };

                    _context.Enrollments.Add(enrollment);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "You have successfully enrolled!";

                    if (redirectToCreate)
                    {
                        return RedirectToAction("CourseSchedule", "Enrollments");
                    }
                }
            }
            else
            {
                return Unauthorized();
            }

            var updatedModel = new EnrollmentViewModel
            {
                AvailableSchedules = GetAvailableSchedules()
            };

            return View(updatedModel);
        }


        public IActionResult CourseSchedule()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrollments = _context.Enrollments
                .Include(e => e.Schedule)
                .Where(e => e.UserId == userId)
                .ToList();

            return View(enrollments);
        }

        private List<SelectListItem> GetAvailableSchedules()
        {
            return _context.Schedules.Select(s => new SelectListItem
            {
                Value = s.ScheduleId.ToString(),
                Text = s.EventName
            }).ToList();
        }

        [HttpGet]
        public IActionResult GetScheduleDetails(int id)
        {
            var schedule = _context.Schedules.Include(s => s.Coach)
                                             .FirstOrDefault(s => s.ScheduleId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return Json(new
            {
                CoachName = $"{schedule.Coach.FirstName} {schedule.Coach.LastName}",
                CoachBio = schedule.Coach.Biography,
                ScheduleDate = schedule.Date,
                schedule.EventName,
                schedule.Location
            });
        }
    }
}
