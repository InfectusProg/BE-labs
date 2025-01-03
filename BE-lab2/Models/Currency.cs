using System.ComponentModel.DataAnnotations;

namespace BE_lab2.Models;

public class Currency : BaseModel
{
    [Required]
    public string Name { get; set; }
}
