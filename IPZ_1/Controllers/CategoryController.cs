﻿using IPZ_1.Data;
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
    }
}
