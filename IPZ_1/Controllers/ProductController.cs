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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Data.Entity;
using IPZ_1.Hubs;

namespace IPZ_1.Controllers
{
	public class ProductController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IHubContext<NotificationHub> _hubContext;

		public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHubContext<NotificationHub> hubContext)
		{
			_db = db;
			_webHostEnvironment = webHostEnvironment;
			_hubContext = hubContext;

		}

		public IActionResult Index()
		{
			IEnumerable<Product> objList = _db.Product.Include(u=>u.Category);
			return View(objList);
		}


		// GET - CREATE/EDIT
		public IActionResult Upsert(int? id)
		{

			

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

                // return view of product with data
                Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "GET-CREATE/EDIT"), WC.logsFile, typeof(List<Logs>));
                return View(productVM);
			}
			else
			{
				// edit
				productVM.Product = _db.Product.Find(id);
				if (productVM.Product == null)
				{
					return NotFound();
				}
                // return view of product with data
                Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "GET-CREATE/EDIT  " + productVM.Product.Name), WC.logsFile, typeof(List<Logs>));
                return View(productVM);
			}
		}

		// POST - CREATE/EDIT
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(PoductVM poductVM)
		{

			await _hubContext.Clients.All.SendAsync("RefreshProducts");
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

                Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "POST-CREATE/EDIT  " + poductVM.Product.Name), WC.logsFile, typeof(List<Logs>));
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
		public async Task<IActionResult> Delete(int? ID)
		{

			await _hubContext.Clients.All.SendAsync("RefreshProducts");

			if (ID == null || ID == 0)
			{
				return NotFound();
			}

			Product product = _db.Product.Include(u=>u.Category).FirstOrDefault(u=>u.Id == ID);

			if (product == null)
			{
				return NotFound();
			}

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "GET-DELETE  " + product.Name), WC.logsFile, typeof(List<Logs>));
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

            Serialize.AddLogAction<Logs>(new Logs(DateTime.Now.ToString(), User.Identity.Name, "POST-DELETE  " + obj.Name), WC.logsFile, typeof(List<Logs>));
            return RedirectToAction("Index");
		}

	}
}
