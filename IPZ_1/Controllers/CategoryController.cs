using IPZ_1.Data;
using Microsoft.AspNetCore.Mvc;
using IPZ_1.Models;
using System.Collections.Generic;
using System;

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
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | GET-CREATE"), WC.logsFile, typeof(List<Logs>));

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
                Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | POST-CREATE  CategoryName:" + obj.Name), WC.logsFile, typeof(List<Logs>));

                return RedirectToAction("Index");
			}
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | POST-CREATE"), WC.logsFile, typeof(List<Logs>));

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

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | GET-EDIT"), WC.logsFile, typeof(List<Logs>));

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
                Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | POST-EDIT"), WC.logsFile, typeof(List<Logs>));


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

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | GET-DELETE"), WC.logsFile, typeof(List<Logs>));

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
            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "CategoryController | POST-DELETE  Deleted category" + obj.Name), WC.logsFile, typeof(List<Logs>));

            return RedirectToAction("Index");
		}

	}
}
