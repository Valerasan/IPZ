namespace IPZ_1.Models.ViewModels
{
	public class DetailsVM
	{
		public DetailsVM() 
		{
			Product = new Product();
		}

		public Product Product { get; set; }
        public bool InFavorite { get; set; }
    }
}
