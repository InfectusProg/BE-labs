using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_lab2.Models
{
    public class User : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
