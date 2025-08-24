
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WooriOptical.Models;

namespace WooriOptical.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Get all user accounts
        var users = _userManager.Users.ToList();
        var accounts = new List<Account>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            accounts.Add(new Account
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = string.Empty,
                Role = roles.FirstOrDefault()
            });
        }

        return View(accounts);
    }

    [HttpGet]
    public IActionResult CreateAccount()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount(Account newAccount)
    {
        if (!ModelState.IsValid)
            return View(newAccount);

        var user = new Account { UserName = newAccount.UserName, Email = newAccount.Email, Password = newAccount.Password };
        var result = await _userManager.CreateAsync(user, user.Password);
        if (result.Succeeded)
        {
            // Assign role if specified
            if (!string.IsNullOrEmpty(newAccount.Role))
            {
                await _userManager.AddToRoleAsync(user, newAccount.Role);
            }
            TempData["Message"] = "Account created successfully.";
            return RedirectToAction("Index");
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(newAccount);
    }

    [HttpGet]
    public async Task<IActionResult> EditAccount(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            TempData["Message"] = "Invalid user ID.";
            return RedirectToAction("Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["Message"] = "User not found.";
            return RedirectToAction("Index");
        }

        var account = new Account
        {
            UserName = user.UserName,
            Email = user.Email,
            Password = string.Empty, // Password should not be pre-filled
            Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
        };

        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAccount(Account updatedAccount)
    {
        if (!ModelState.IsValid)
            return View(updatedAccount);

        var user = await _userManager.FindByNameAsync(updatedAccount.UserName);
        if (user == null)
        {
            TempData["Message"] = "User not found.";
            return RedirectToAction("Index");
        }

        user.Email = updatedAccount.Email;
        if (!string.IsNullOrEmpty(updatedAccount.Password))
        {
            var passwordResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, updatedAccount.Password);
            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(updatedAccount);
            }
        }

        var roleResult = await _userManager.RemoveFromRoleAsync(user, (await _userManager.GetRolesAsync(user)).FirstOrDefault());
        if (roleResult.Succeeded && !string.IsNullOrEmpty(updatedAccount.Role))
        {
            await _userManager.AddToRoleAsync(user, updatedAccount.Role);
        }

        TempData["Message"] = "Account updated successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccount(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            TempData["Message"] = "Invalid user ID.";
            return RedirectToAction("Index");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["Message"] = "User not found.";
            return RedirectToAction("Index");
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["Message"] = "User deleted successfully.";
        }
        else
        {
            TempData["Message"] = "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
        }

        return RedirectToAction("Index");
    }
}