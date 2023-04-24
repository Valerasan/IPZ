using IPZ_1.Data;
using IPZ_1.Hubs;
using IPZ_1.Models;
using IPZ_1.Models.ViewModels;
using IPZ_1.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IPZ_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
		private readonly IHubContext<NotificationHub> _hubContext;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _db = db;
			_hubContext = hubContext;
		}

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _db.Product.Include(u => u.Category),
                Categories = _db.Category
            };
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "HomeController | GET-INDEX"), WC.logsFile, typeof(List<Logs>));

            return View(homeVM);
        }

		[HttpPost]
		public async Task<IActionResult> Index(HomeVM model)
		{

			await _hubContext.Clients.All.SendAsync("RefreshProducts");
			return View();
		}


		public IActionResult Logs()
        {
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "HomeController | GET-LOGS"), WC.logsFile, typeof(List<Logs>));

            List<Logs> logs = new List<Logs>();
            logs = Serialize.DeserializeJson<Logs>(WC.logsFile, typeof(List<Logs>));
            return View(logs);
        }


        // GET 
        public IActionResult Details(int id)
        {
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "HomeController | GET-DETAILS"), WC.logsFile, typeof(List<Logs>));


            List<Favorite> favoriteList = new List<Favorite>();
            if (HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite) != null
                && HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite).Count() > 0)
            {
                favoriteList = HttpContext.Session.Get<List<Favorite>>(WC.sessionFavorite);
            }

            
            

                DetailsVM DetailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Where(u => u.Id == id).FirstOrDefault(),
                InFavorite = false
            };


            foreach (var item in favoriteList)
            {
                if (item.ProductId == id)
                {
                    DetailsVM.InFavorite = true;
                }
            }

            return View(DetailsVM);
        }

        // POST

        [HttpPost, ActionName("Details")]
        public async Task<IActionResult> DetailsPost(int id)
        {
			await _hubContext.Clients.All.SendAsync("RefreshProducts");


			List<Favorite> favoriteList = new List<Favorite>();
            if (HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite) != null
                && HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite).Count() > 0)
            {
                favoriteList = HttpContext.Session.Get<List<Favorite>>(WC.sessionFavorite);
            }
            favoriteList.Add(new Favorite { ProductId = id });
            HttpContext.Session.Set(WC.sessionFavorite, favoriteList);

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "HomeController | POST-DETAILS"), WC.logsFile, typeof(List<Logs>));


            return RedirectToAction(nameof(Index));
        }


        public IActionResult RemoveFromFavorites(int id)
        {
            List<Favorite> favoriteList = new List<Favorite>();
            if (HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite) != null
                && HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite).Count() > 0)
            {
                favoriteList = HttpContext.Session.Get<List<Favorite>>(WC.sessionFavorite);
            }
            var itemToRemove = favoriteList.SingleOrDefault(f => f.ProductId == id);
            if (itemToRemove != null)
            {
                favoriteList.Remove(itemToRemove);
            }

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "HomeController | GET-RemoveFromFavorites  ProductId:" + itemToRemove.ProductId), WC.logsFile, typeof(List<Logs>));

            HttpContext.Session.Set(WC.sessionFavorite, favoriteList);
            return RedirectToAction(nameof(Index));
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
