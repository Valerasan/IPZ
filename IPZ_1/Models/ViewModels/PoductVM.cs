using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace IPZ_1.Models.ViewModels
{
	public class PoductVM
	{
		public Product Product { get; set; }
		public IEnumerable<SelectListItem> CategotySelectList { get; set;}
	}
}
