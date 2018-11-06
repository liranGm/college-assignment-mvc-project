using System;
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
            // TODO: There is a bug, when new user registers to the site, we do insert him to DB but not logging him in. we must do it in Create Method in this controller
            //if (HttpContext.Session.GetString("Role") != "ADMIN")
            //{
            //    // TODO: Fix this tempData msg name to more relevant onegit s
            //    TempData["msg"] = "<script>alert('Requested page is not available for you..');</script>";
            //    return RedirectToAction("Index", "Home");
            //}
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Role") != "ADMIN")
            {
                TempData["msg"] = "<script>alert('Requested page is not available for you..');</script>";
                return RedirectToAction("Index", "Home");
            }
            else
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

                return View(user);
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,FirstName,LastName,PhoneNumber,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
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
            
            if (HttpContext.Session.GetString("IsUserLoggedIn") != "UserConnected" || HttpContext.Session.GetString("UserFirstName") != user.FirstName)
            {
                TempData["msg"] = "<script>alert('OOps.. You must be logged in to edit your profile');</script>";
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,FirstName,LastName,PhoneNumber")] User user)
        {
            if (HttpContext.Session.GetString("IsUserLoggedIn") != "UserConnected")
            {
                TempData["msg"] = "<script>alert('OOps.. You must be logged in to edit your profile');</script>";
                return RedirectToAction("Index", "Home");
            }
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
            if (HttpContext.Session.GetString("IsUserLoggedIn") != "UserConnected" || HttpContext.Session.GetString("UserFirstName") != user.FirstName)
            {
                TempData["msg"] = "<script>alert('OOps.. You must be logged in to edit your profile');</script>";
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        public ActionResult Login([Bind(include: "Email,Password")] User user)
        {
            if (HttpContext.Session.GetString("IsUserLoggedIn") == "UserConnected" )
            {
                TempData["msg"] = "<script>alert('OOps.. You must log out before sign in again..');</script>";
                return RedirectToAction("Index", "Home");
            }

            User usr = null;
            var password = user.Password;
            var email = user.Email;

            try
            {
                usr = _context.User.Single(u => u.Email.Equals(email) && u.Password.Equals(password));
                if (usr != null)
                {
                    HttpContext.Session.SetString("UserFirstName", usr.FirstName);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("FailedLogin", "Users");
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }
    }
}
