
using System.Collections.Generic;

namespace IPZ_1.Models.ViewModels
{
	public class HomeVM
	{
		public IEnumerable<Product> Products { get; set; }
		public IEnumerable<Category> Categories { get; set;}
	}
}
