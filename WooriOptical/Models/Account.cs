using Microsoft.AspNetCore.Identity;

namespace WooriOptical.Models;

public class Account : IdentityUser
{
    public required string Password { get; set; }
    public string? Role { get; set; }
}