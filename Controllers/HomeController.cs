using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using college_assignment_mvc_project.Models;

namespace college_assignment_mvc_project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search()
        {
            ViewData["Message"] = "Your search page.";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Your login page.";

            return View();
        }

        public IActionResult SignUp()
        {
            ViewData["Message"] = "Your sign up page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
