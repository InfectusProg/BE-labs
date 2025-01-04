namespace BE_lab2.Contracts;

using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    public string? UserName { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
