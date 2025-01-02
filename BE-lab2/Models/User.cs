using System.ComponentModel.DataAnnotations;

namespace BE_lab2.Models
{
    public class User : BaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
