using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using college_assignment_mvc_project.Models;
using Microsoft.AspNetCore.Http;
using college_assignment_mvc_project.Models.ViewModels;

namespace college_assignment_mvc_project.Controllers
{
    public class GuidesController : Controller
    {
        private readonly college_assignment_mvc_projectContext _context;

        private IActionResult redirect_to_login_page()
        {
            TempData["must-login-msg"] = "<p>Must log in to see this page</p>";
            return RedirectToAction("Login", "Home");
        }
        public GuidesController(college_assignment_mvc_projectContext context)
        {
            _context = context;
        }

        // GET: Guides
        public async Task<IActionResult> Index()
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            return View(await _context.Guide.ToListAsync());
        }

        // GET: Guides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            else if (!(AuthorizationMiddleware.IsGuideAuthorized(HttpContext.Session) 
                        || AuthorizationMiddleware.IsAdminAuthorized(HttpContext.Session)))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide
                .FirstOrDefaultAsync(m => m.GuideID == id);
            if (guide == null)
            {
                return NotFound();
            }

            return View(guide);
        }

        // GET: Guides/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GuideID,UserID,FirstName,LastName,PricePerDay,Rate")] Guide guide)
        {
            if (ModelState.IsValid)
            {
                _context.Guide.Add(guide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guide);
        }

        // GET: Guides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }
            return View(guide);
        }

        // POST: Guides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GuideID,UserID,FirstName,LastName,PricePerDay,Rate")] Guide guide)
        {
            if (id != guide.GuideID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuideExists(guide.GuideID))
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
            return View(guide);
        }

        // GET: Guides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide
                .FirstOrDefaultAsync(m => m.GuideID == id);
            if (guide == null)
            {
                return NotFound();
            }

            return View(guide);
        }

        // POST: Guides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guide = await _context.Guide.FindAsync(id);
            _context.Guide.Remove(guide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuideExists(int id)
        {
            return _context.Guide.Any(e => e.GuideID == id);
        }

        public IActionResult GetGuidesStats()
        {
            var orders = (from order in _context.Order
                          join guide in _context.Guide on order.SelectedGuildeID equals guide.GuideID
                          select new OrderedGuide
                          {
                              TimesOrderd = 1,
                              GuideName = guide.FirstName + " " + guide.LastName,
                              GuideID = guide.GuideID
                          }).ToList();

            var ordered_guides = new Dictionary<int, OrderedGuide>();
            var guides_ids = (from guide in _context.Guide select guide.GuideID).ToList();

            foreach (var guide_id in guides_ids)
                ordered_guides[guide_id] = new OrderedGuide { GuideID = guide_id, TimesOrderd = 0, GuideName = "" };

            foreach (var order in orders)
            {
                ordered_guides[order.GuideID].TimesOrderd += 1;
                ordered_guides[order.GuideID].GuideName = order.GuideName;
            }

            List<OrderedGuide> orderd_guides = new List<OrderedGuide>(ordered_guides.Count);
            foreach (var ordered_guide in ordered_guides.Values)
            {
                if (ordered_guide.TimesOrderd > 0)
                    orderd_guides.Add(ordered_guide);
            }

            return Json(orderd_guides);
        }

        public IActionResult Stats()
        {
            return View();
        }
    }
}
