using IPZ_1.Data;
using Microsoft.AspNetCore.Mvc;
using IPZ_1.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using IPZ_1.Models.ViewModels;

namespace IPZ_1.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;

		public ProductController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> objList = _db.Product;

			foreach (var obj in objList)
			{
				obj.Category = _db.Category.FirstOrDefault(u => u.ID == obj.CategoryId);

			}

			return View(objList);
		}


		// GET - CREATE
		public IActionResult Upsert(int? id)
		{
			//IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
			//{
			//	Text = i.Name,
			//	Value = i.ID.ToString()
			//});

			////ViewBag.CategoryDropDown = CategoryDropDown;
			//ViewData["CategoryDropDown"] = CategoryDropDown;

			//IEnumerable<Category> student = _db.Category;
			//Test.SerializeJson<Category>("test", student);




			//Product product = new Product();

			PoductVM productVM = new PoductVM()
			{
				Product = new Product(),
				CategotySelectList = _db.Category.Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.ID.ToString()
				})
			};



			if (id == null)
			{
				// creation
				return View(productVM);
			}
			else
			{
				productVM.Product = _db.Product.Find(id);
				if (productVM.Product == null)
				{
					return NotFound();
				}
				return View(productVM);
			}
		}

		// POST - CREATE
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(Category obj)
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
