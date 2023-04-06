using IPZ_1.Data;
using Microsoft.AspNetCore.Mvc;
using IPZ_1.Models;
using System.Collections.Generic;

namespace IPZ_1.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Category> objList = _db.Category;
			return View(objList);
		}


		// GET - CREATE
		public IActionResult Create()
		{
			return View();
		}

		// POST - CREATE
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Category obj)
		{
			//server validation
			if (ModelState.IsValid)
			{
				_db.Category.Add(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}


		// GET - EDIT
		public IActionResult Edit(int ID)
		{
			if (ID == null || ID == 0)
			{
				return NotFound();
			}
			var obj = _db.Category.Find(ID);
			if (obj == null)
			{
				return NotFound();
			}


			return View(obj);
		}


		// POST - EDIT
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category obj)
		{
			//server validation
			if (ModelState.IsValid)
			{
				_db.Category.Update(obj);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(obj);
		}

		// GET - Delete
		public IActionResult Delete(int ID)
		{
			if (ID == null || ID == 0)
			{
				return NotFound();
			}
			var obj = _db.Category.Find(ID);
			if (obj == null)
			{
				return NotFound();
			}


			return View(obj);
		}


		// POST - Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? ID)
		{
			var obj = _db.Category.Find(ID);
			if (obj == null) 
			{
				return NotFound();
			}
			_db.Category.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}
