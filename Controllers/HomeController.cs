using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using college_assignment_mvc_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Accord.MachineLearning.Bayes;

namespace college_assignment_mvc_project.Controllers
{

    public class HomeController : Controller
    {
        private readonly college_assignment_mvc_projectContext _context;

        public HomeController(college_assignment_mvc_projectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserFirstName") == null)
            {
                HttpContext.Session.SetString("UserFirstName", "Guest");
                HttpContext.Session.SetString("IsUserLoggedIn", "UserNotConnected");
            }

            Track recomended_track = RecommendTrack();
            if (recomended_track.TrackID != 0)
                ViewData["RecomendedTrack"] = recomended_track;

            return View(await _context.Track.ToListAsync());

        }

        public IActionResult Search(string search)
        {
            IQueryable<Track> result = null;
            result = _context.Track
                .Where(t => 
                    t.Name.Contains(search) || 
                    t.Location.Contains(search)
                );

            ViewData["Message"] = search;
            ViewData["Result"] = result;

            return View();
        }

        public async Task<IActionResult> Cart()
        {
            var userID = Int32.Parse(HttpContext.Session.GetString("UserID"));

        
            List<Track> allTracksInArea = new List<Track>();
            int ordersLength = _context.Order.Count();
            if (ordersLength == 0 || !AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
            {
                
            }
            else
            {
                var userID2 = Int32.Parse(HttpContext.Session.GetString("UserID"));
                var track_for_this_user = _context.Order.Where(user => user.UserID == userID2).Select(order => order.PurchasedTrackID).ToList();

               // List<string> selectedAreaList = new List<string>();

                Dictionary<int, string> dictionary = new Dictionary<int, string>();

                foreach (var trackID in track_for_this_user)
                {
                    string area = "";
                    try
                    {
                        area = _context.Track.Where(x => x.TrackID == trackID).First().Name;
                        dictionary.Add(trackID, area);
                    }
                    catch
                    {
                    }
       
                }
                ViewData["dictionary"] = dictionary;
            }

            return View(await _context.Order.Where(o => o.UserID == userID).ToListAsync());
        }


        public IActionResult Login()
        {
            ViewData["Message"] = "Your login page.";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
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


        public IActionResult UnauthorizedAction()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(include: "Email,Password")] User user)
        {
            User usr = null;
            var password = user.Password;
            var email = user.Email;
            try
            {
                usr = _context.User.Single(u => u.Email.Equals(email) && u.Password.Equals(password));
                HttpContext.Session.SetString("UserFirstName", usr.FirstName);
                HttpContext.Session.SetString("Role", usr.Role.ToString());
                HttpContext.Session.SetString("IsUserLoggedIn", "UserConnected");
                HttpContext.Session.SetString("UserID", usr.UserID.ToString());

                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException)
            {
                TempData["login-failure"] = "login-failure";
                return RedirectToAction("Login", "Home");
            }
        }

        public Track RecommendTrack()
        {
            Random rnd = new Random();
            Track selectedTrack = null;
            List<Track> allTracksInArea = new List<Track>();
            int ordersLength = _context.Order.Count();

            if (ordersLength == 0 || !AuthorizationMiddleware.IsUserLoggedIn(HttpContext.Session))
            {
                return new Track();
            }
            else
            {
                int userID = Int32.Parse(HttpContext.Session.GetString("UserID"));
                var track_for_this_user = _context.Order.Where(user => user.UserID == userID).Select(order => order.PurchasedTrackID).ToList();

                List<string> selectedAreaList = new List<string>();
                foreach (var trackID in track_for_this_user)
                {
                    string area = "";
                    try
                    {
                        area = _context.Track.Where(x => x.TrackID == trackID).First().Location;
                    }
                    catch
                    {
                    }
                    selectedAreaList.Add(area);
                }

                string[] selectedArea = selectedAreaList.ToArray();

                if (selectedArea.Length == 0)
                    return new Track();

                else if (selectedArea.Distinct().Count() == 1)
                {
                    try
                    {
                        string area = selectedArea[0];
                        allTracksInArea.Add(_context.Track.First(x => x.Location.Equals(area)));
                        selectedTrack = allTracksInArea[rnd.Next(allTracksInArea.Count)];
                    }
                    catch
                    {
                        return new Track();
                    }
                }
                else
                {
                    int[][] dataset = new int[selectedArea.Length][];

                    Dictionary<int, int> mapper = new Dictionary<int, int>();
                    Dictionary<int, int> mapperOpsite = new Dictionary<int, int>();

                    int counter = 0;

                    for (int genereIndex = 0; genereIndex < selectedArea.Length; genereIndex++)
                    {
                        dataset[genereIndex] = new int[1];

                        if (!mapper.ContainsKey(get_selected_area_value(selectedArea[genereIndex])))
                        {
                            mapper[get_selected_area_value(selectedArea[genereIndex])] = counter;
                            mapperOpsite[counter] = get_selected_area_value(selectedArea[genereIndex]);

                            counter++;
                        }

                    }

                    int[] mappedLabels = new int[selectedArea.Length];

                    for (int i = 0; i < selectedArea.Length; i++)
                    {
                        mappedLabels[i] = mapper[get_selected_area_value(selectedArea[i])];
                    }


                    var learner = new NaiveBayesLearning();
                    NaiveBayes nb = learner.Learn(dataset, mappedLabels);

                    int[] prediction = new int[] { default(int) };

                    int selectedGenereMapped = nb.Decide(prediction);

                    int selectedIndex = mapperOpsite[selectedGenereMapped];

                    var relevant_tracks = _context.Track.Where(x => get_selected_area_value(x.Location) == selectedIndex);
                    foreach (var track in relevant_tracks)
                        allTracksInArea.Add(track);

                    selectedTrack = allTracksInArea[rnd.Next(allTracksInArea.Count)];
                }
            }
            
            return selectedTrack;
        }

        protected int get_selected_area_value(string area)
        {
            switch (area)
            {
            case "Tzafon":
                return 0;
            case "Merkaz":
                return 1;
            case "Darom":
                return 2;
            default:
                return -999;
            }
        }
    }
}
