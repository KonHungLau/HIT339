using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnyoneForTennis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Offers = _context.SpecialOffers.ToList();
            return View();
        }




        public IActionResult CoachList()
        {

            List<Coach> coaches = _context.Coaches.ToList();

            return View(coaches);
        }
        public IActionResult CoachDetails(string id)
        {
            // attempt to find coach by id
            Coach coach = _context.Coaches.FirstOrDefault(c => c.CoachId == id);

            if (coach == null)
            {
                // if coach is not found, return 404 not found error
                return NotFound();
            }

            return View(coach);
        }
    }
}
