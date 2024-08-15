using System.ComponentModel.DataAnnotations;

namespace PaySpace.Calculator.Web.Models;

public sealed class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "dimitrycode@gmail.com";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "!v3ryS3cur3d";
}