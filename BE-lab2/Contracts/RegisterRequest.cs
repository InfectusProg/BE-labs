using System.ComponentModel.DataAnnotations;

namespace BE_lab2;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public int? CurrencyId { get; set; }
}