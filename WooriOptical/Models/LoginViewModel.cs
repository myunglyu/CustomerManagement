using System.ComponentModel.DataAnnotations;

namespace WooriOptical.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "User ID")]
    public string UserName { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
