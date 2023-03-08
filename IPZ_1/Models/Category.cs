using System.ComponentModel.DataAnnotations;

namespace IPZ_1.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayOrder { get; set; }


    }
}
