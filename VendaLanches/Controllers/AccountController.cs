using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VendaLanches.ViewModels;

namespace VendaLanches.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel()
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        var user = await _userManager.FindByNameAsync(loginVM.UserName);

        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    return RedirectToAction("Index", "Home");

                return Redirect(loginVM.ReturnUrl);
            }
        }

        ModelState.AddModelError("", "Falha ao realizar o login.");
        return View(loginVM);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(LoginViewModel registroVM)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = registroVM.UserName };
            var result = await _userManager.CreateAsync(user, registroVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError("Registro", "Falha ao registrar o usuário.");
            }
        }
        return View(registroVM);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.User = null;
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
