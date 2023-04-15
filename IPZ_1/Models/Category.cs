using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace IPZ_1.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Display Order must be greater then 0")]
        public string DisplayOrder { get; set; }

    }
}
