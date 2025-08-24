using Microsoft.AspNetCore.Identity;

namespace WooriOptical.Models;

public class AccountViewModel : IdentityUser
{
    public string? Password { get; set; }
    public string? Role { get; set; }
}