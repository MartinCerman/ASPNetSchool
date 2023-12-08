using ASP.NetSchool.Models;
using ASP.NetSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetSchool.Controllers;

public class AccountController : Controller {
	private readonly UserManager<AppUser> userManager;
	private readonly SignInManager<AppUser> signInManager;

	public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
		this.userManager = userManager;
		this.signInManager = signInManager;
	}

	public IActionResult Login(string returnUrl) {
		LoginViewModel login = new() {
			ReturnUrl = returnUrl,
		};
		return View(login);
	}

	[HttpPost]
	[AllowAnonymous]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginViewModel login) {
		if (ModelState.IsValid) {
			AppUser appUser = await userManager.FindByNameAsync(login.Username);
			if (appUser != null) {
				var signInResult = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
				if (signInResult.Succeeded) {
					return Redirect(login.ReturnUrl ?? "/");
				}
			}
			ModelState.AddModelError("", "Login failed: Invalid username or password");
		}
		return View(login);
	}

	public async Task<IActionResult> Logout() {
		await signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}

	public ActionResult AccessDenied() {
		return View();
	}
}
