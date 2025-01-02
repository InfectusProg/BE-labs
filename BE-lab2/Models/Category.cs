using System.ComponentModel.DataAnnotations;

namespace BE_lab2.Models;
public class Category : BaseModel
{
    [Required]
    public string CategoryName { get; set; }
}
