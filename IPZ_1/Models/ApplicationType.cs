using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace IPZ_1.Models
{
    public class ApplicationType
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

    }
}
