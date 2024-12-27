using HaberPortal.Models;
using HaberPortal.RepositoryInterfaces;
using HaberPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

// AdminController
[Authorize(Roles = ("Admin"))]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        ViewData["Layout"] = "~/Views/Shared/_LayoutAdmin.cshtml";
    }

    [HttpGet]
    public IActionResult AdminPanel()
    {
        // Admin paneli view'ını döndürür.
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Users()
    {
        var users = _userManager.Users.ToList();
        var userRoles = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles.Add(new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles
            });
        }

        return View(userRoles);
    }

    [HttpGet]
    public async Task<IActionResult> ManageRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var roles = _roleManager.Roles.ToList();
        var userRoles = await _userManager.GetRolesAsync(user);

        var model = new ManageRolesViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            Roles = roles.Select(r => new RoleViewModel
            {
                RoleName = r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ManageRoles(ManageRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();

        var result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Rolleri kaldırırken bir hata oluştu.");
            return View(model);
        }

        result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Rolleri eklerken bir hata oluştu.");
            return View(model);
        }

        return RedirectToAction("Users");
    }
}
