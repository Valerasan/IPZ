﻿using IPZ_1.Data;
using IPZ_1.Models;
using IPZ_1.Models.ViewModels;
using IPZ_1.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
			_logger = logger;
			_db = db;

		}

		public IActionResult Index()
		{
			HomeVM homeVM = new HomeVM()
			{
				Products = _db.Product.Include(u => u.Category),
				Categories = _db.Category
			};

			return View(homeVM);
		}

		// GET
		public IActionResult Details(int id)
		{
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
                if(item.ProductId == id)
				{
					DetailsVM.InFavorite = true;
				}
            }

            return View(DetailsVM);
		}

		// POSST

		[HttpPost, ActionName("Details")]
		public IActionResult DetailsPost(int id)
		{
			List<Favorite> favoriteList = new List<Favorite>();
			if (HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite) != null 
				&& HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite).Count() > 0)
			{
				favoriteList = HttpContext.Session.Get<List<Favorite>>(WC.sessionFavorite);
			}
			favoriteList.Add(new Favorite { ProductId = id });
			HttpContext.Session.Set(WC.sessionFavorite, favoriteList);

			return RedirectToAction(nameof(Index));
		}

		
		public IActionResult RemoveFromCart(int id)
		{
			List<Favorite> favoriteList = new List<Favorite>();
			if (HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite) != null
				&& HttpContext.Session.Get<IEnumerable<Favorite>>(WC.sessionFavorite).Count() > 0)
			{
				favoriteList = HttpContext.Session.Get<List<Favorite>>(WC.sessionFavorite);
			}
			var itemToRemove = favoriteList.SingleOrDefault(f => f.ProductId == id);
			if(itemToRemove != null) 
			{
				favoriteList.Remove(itemToRemove);
			}

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
