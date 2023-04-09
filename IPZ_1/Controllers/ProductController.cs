using IPZ_1.Data;
using Microsoft.AspNetCore.Mvc;
using IPZ_1.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using IPZ_1.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace IPZ_1.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;

        }

		public IActionResult Index()
		{
			IEnumerable<Product> objList = _db.Product.Include(u=>u.Category);

			//foreach (var obj in objList)
			//{
			//	obj.Category = _db.Category.FirstOrDefault(u => u.ID == obj.CategoryId);

			//}

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
		public IActionResult Upsert(PoductVM poductVM)
		{
			//server validation
			if (ModelState.IsValid)
			{
				var files = HttpContext.Request.Form.Files;
				string webRootPath = _webHostEnvironment.WebRootPath;


				if(poductVM.Product.Id == 0) 
				{
					// create
					string upload = webRootPath + WC.ImagePath;
					string fileName = Guid.NewGuid().ToString();
					string extension = Path.GetExtension(files[0].FileName);

					using(var fileStream = new FileStream(Path.Combine(upload, fileName+ extension), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}

					poductVM.Product.Image = fileName + extension;

					_db.Product.Add(poductVM.Product);

				}
				else
				{
					// update

					var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == poductVM.Product.Id);

					if(files.Count > 0)
					{
						string upload = webRootPath + WC.ImagePath;
						string fileName = Guid.NewGuid().ToString();
						string extension = Path.GetExtension(files[0].FileName);

						var oldFile = Path.Combine(upload, objFromDb.Image);
						if(System.IO.File.Exists(oldFile))
						{
							System.IO.File.Delete(oldFile);
						}

						using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
						{
							files[0].CopyTo(fileStream);
						}
						poductVM.Product.Image = fileName + extension;

					}
					else
					{
						poductVM.Product.Image = objFromDb.Image;

					}
					_db.Product.Update(poductVM.Product);
				}

				_db.SaveChanges();
				return RedirectToAction("Index");
			}


			poductVM.CategotySelectList = _db.Category.Select(i => new SelectListItem
			{
				Text = i.Name,
				Value = i.ID.ToString()
			});
			return View(poductVM);
		}


		// GET - Delete
		public IActionResult Delete(int? ID)
		{
			if (ID == null || ID == 0)
			{
				return NotFound();
			}

			Product product = _db.Product.Include(u=>u.Category).FirstOrDefault(u=>u.Id == ID);
			//product.Category = _db.Category.Find(product.Id);


			if (product == null)
			{
				return NotFound();
			}


			return View(product);
		}


		// POST - Delete
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? ID)
		{
			var obj = _db.Product.Find(ID);
			if (obj == null)
			{
				return NotFound();
			}

			string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
			var oldFile = Path.Combine(upload, obj.Image);

			if (System.IO.File.Exists(oldFile))
			{
				System.IO.File.Delete(oldFile);
			}

			_db.Product.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}
