﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using college_assignment_mvc_project.Models;
using Microsoft.AspNetCore.Http;

namespace college_assignment_mvc_project.Controllers
{
    public class UsersController : Controller
    {
        private readonly college_assignment_mvc_projectContext _context;

        public UsersController(college_assignment_mvc_projectContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (AuthorizationMiddleware.IsAdminAuthorized(HttpContext.Session) 
                && AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return View(await _context.User.ToListAsync());
            return RedirectToAction("UnauthorizedAction", "Home");
        }

        // GET: Users/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "ADMIN")
            {
                return RedirectToAction("UnauthorizedAction", "Home");
            }
            else
            {
                if (id == null)
                    return NotFound();

                var user = await _context.User.FirstOrDefaultAsync(m => m.UserID == id);
                if (user == null)
                    return NotFound();
                
                return View(user);
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
                return View();
            return RedirectToAction("UnauthorizedAction", "Home");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,PhoneNumber")] User user)
        {
            if (!AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
            {
                if (ModelState.IsValid)
                {
                    user.Role = UserAuthorization.USER;
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    // When you create a user we beleive he will automaticly sign in
                    HttpContext.Session.SetString("UserFirstName", user.FirstName);
                    HttpContext.Session.SetString("Role", user.Role.ToString());
                    HttpContext.Session.SetString("IsUserLoggedIn", "UserConnected");
                }
                else
                    return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);            
            if (user == null)
            {
                return NotFound();
            }
            
            if (HttpContext.Session.GetString("IsUserLoggedIn") == "UserConnected" 
                && (HttpContext.Session.GetString("UserFirstName") == user.FirstName 
                    || HttpContext.Session.GetString("Role") == "ADMIN"))
            {
                return View(user);
            }
            return RedirectToAction("UnauthorizedAction", "Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,PhoneNumber,Role")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    var newGuide = new Guide
                    {
                        UserID = user.UserID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Rate = 3,
                        PricePerDay = 100,
                    };

                    _context.Guide.Add(newGuide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }
            
            if (!(AuthorizationMiddleware.IsAdminAuthorized(HttpContext.Session) 
                && AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session)))
            {
                return RedirectToAction("UnauthorizedAction", "Home");
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            var guide = await _context.Guide.FirstOrDefaultAsync(g => g.UserID == id);

            _context.User.Remove(user);
            _context.Guide.Remove(guide);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
