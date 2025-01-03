using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE_lab2.Models;

public class Record : BaseModel
{
    [Display(Name = "User")]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; }

    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }

    public int? CurrencyId { get; set; }
    [ForeignKey("CurrencyId")]
    public virtual Currency Currency { get; set; }
}
