using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Authorization;

namespace AnyoneForTennis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SpecialOffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialOffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SpecialOffers
        public async Task<IActionResult> Index()
        {
              return _context.SpecialOffers != null ? 
                          View(await _context.SpecialOffers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.SpecialOffers'  is null.");
        }

        // GET: SpecialOffers/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var specialOffer = _context.SpecialOffers.Find(id);
            if (specialOffer == null)
            {
                return NotFound();
            }
            return View(specialOffer);
        }



        // GET: SpecialOffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpecialOffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialOfferId,Title,Description,StartDate,EndDate,TargetAudience")] SpecialOffer specialOffer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(specialOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialOffer);
        }

        // GET: SpecialOffers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SpecialOffers == null)
            {
                return NotFound();
            }

            var specialOffer = await _context.SpecialOffers.FindAsync(id);
            if (specialOffer == null)
            {
                return NotFound();
            }
            return View(specialOffer);
        }

        // POST: SpecialOffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecialOfferId,Title,Description,StartDate,EndDate,TargetAudience")] SpecialOffer specialOffer)
        {
            if (id != specialOffer.SpecialOfferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialOfferExists(specialOffer.SpecialOfferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(specialOffer);
        }

        // GET: SpecialOffers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SpecialOffers == null)
            {
                return NotFound();
            }

            var specialOffer = await _context.SpecialOffers
                .FirstOrDefaultAsync(m => m.SpecialOfferId == id);
            if (specialOffer == null)
            {
                return NotFound();
            }

            return View(specialOffer);
        }

        // POST: SpecialOffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SpecialOffers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SpecialOffers'  is null.");
            }
            var specialOffer = await _context.SpecialOffers.FindAsync(id);
            if (specialOffer != null)
            {
                _context.SpecialOffers.Remove(specialOffer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialOfferExists(int id)
        {
          return (_context.SpecialOffers?.Any(e => e.SpecialOfferId == id)).GetValueOrDefault();
        }

    }
}
