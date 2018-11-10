using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using college_assignment_mvc_project.Models;
using college_assignment_mvc_project.Models.ViewModels;

namespace college_assignment_mvc_project.Controllers
{
    public class OrdersController : Controller
    {
        private readonly college_assignment_mvc_projectContext _context;

        private IActionResult redirect_to_login_page()
        {
            return RedirectToAction("UnauthorizedAction", "Home");
        }

        public OrdersController(college_assignment_mvc_projectContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            var college_assignment_mvc_projectContext = _context.Order.Include(o => o.customer);
            return View(await college_assignment_mvc_projectContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.customer)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");

            if (TempData["TrackID"] != null && TempData["GuideID"] != null)
            {
                ViewData["TrackID"] = TempData["TrackID"];
                ViewData["GuideID"] = TempData["GuideID"];
            }

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,UserID,TotalPrice,PurchasedTrackID,SelectedGuildeID")] Order order)
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return redirect_to_login_page();
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,UserID,TotalPrice,PurchasedTrackID,SelectedGuildeID,PurchaseDate,TripsDate")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.customer)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }

        public IActionResult GetAllOrdersData()
        {
            var orders = (from order in _context.Order
                          join track in _context.Track on order.PurchasedTrackID equals track.TrackID
                          select new OrderdTrip
                          {
                              TimesOrderd = 1,
                              TripName = track.Name,
                              TrackID = track.TrackID
                          }).ToList();

            var ordered_tracks = new Dictionary<int, OrderdTrip>();
            var tracks_ids = (from track in _context.Track select track.TrackID).ToList();

            foreach (var track_id in tracks_ids)
                ordered_tracks[track_id] = new OrderdTrip { TrackID = track_id, TimesOrderd = 0, TripName = "" };

            foreach (var order in orders)
            {
                ordered_tracks[order.TrackID].TimesOrderd += 1;
                ordered_tracks[order.TrackID].TripName = order.TripName;
            }

            List<OrderdTrip> orderd_trips = new List<OrderdTrip>(ordered_tracks.Count);
            foreach (var ordered_trip in ordered_tracks.Values)
                orderd_trips.Add(ordered_trip);

            return Json(orderd_trips);
        }

        public IActionResult Stats()
        {
            return View();
        }
    }
}
