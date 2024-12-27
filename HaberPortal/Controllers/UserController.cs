using HaberPortal.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly IHesapRepository _hesapRepository;

    public UserController(IHesapRepository hesapRepository)
    {
        _hesapRepository = hesapRepository;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (ModelState.IsValid)
        {
            var result = await _hesapRepository.LoginAsync(email, password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Haber");
            }
            ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        }
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string fullName)
    {
        if (ModelState.IsValid)
        {
            var result = await _hesapRepository.RegisterAsync(email, password, fullName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Haber");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _hesapRepository.LogoutAsync();
        return RedirectToAction("Index", "Haber");
    }
}