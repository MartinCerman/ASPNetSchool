using ASP.NetSchool.Models;
using ASP.NetSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetSchool.Controllers;

//[Authorize(Roles = "Admin")]
public class UsersController : Controller {
	private UserManager<AppUser> userManager;
	private IPasswordHasher<AppUser> passwordHasher;
	private IPasswordValidator<AppUser> passwordValidator;

	public UsersController(
		UserManager<AppUser> userManager,
		IPasswordHasher<AppUser> passwordHasher,
		IPasswordValidator<AppUser> passwordValidator) {

		this.userManager = userManager;
		this.passwordHasher = passwordHasher;
		this.passwordValidator = passwordValidator;
	}

	public IActionResult Index() {
		return View(userManager.Users);
	}

	public IActionResult Create() {
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(UserViewModel userViewModel) {
		if (ModelState.IsValid) {
			AppUser newUser = new() {
				UserName = userViewModel.Name,
				Email = userViewModel.Email,
			};

			var result = await userManager.CreateAsync(newUser, userViewModel.Password);

			if (result.Succeeded) {
				return RedirectToAction("Index");
			}
			else {
				foreach (var error in result.Errors) {
					ModelState.AddModelError("", error.Description);
				}
			}
		}

		return View(userViewModel);
	}

	public async Task<IActionResult> Edit(string id) {
		AppUser userToEdit = await userManager.FindByIdAsync(id);

		if (userToEdit == null) {
			return View("NotFound");
		}
		else {
			return View(userToEdit);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Edit(string id, string email, string password) {
		AppUser userToEdit = await userManager.FindByIdAsync(id);
		bool isPasswordValid = true;

		if (userToEdit is null) {
			ModelState.AddModelError("", "User not found");
			return View();
		}
		else if (password is null || email is null) {
			return View(userToEdit);
		}
		else {
			IdentityResult? validPasswordResult
				= await passwordValidator.ValidateAsync(userManager, userToEdit, password);
			if (!validPasswordResult.Succeeded) {
				isPasswordValid = false;
				foreach (IdentityError? error in validPasswordResult.Errors) {
					ModelState.AddModelError("", error.Description);
				}
			}

			//foreach (IPasswordValidator<AppUser>? validator in userManager.PasswordValidators) {
			//    IdentityResult passwordResult = await validator.ValidateAsync(userManager, userToEdit, password);
			//    if (!passwordResult.Succeeded) {
			//        isPasswordValid = false;

			//        foreach (IdentityError? error in passwordResult.Errors) {
			//            ModelState.AddModelError("", error.Description);
			//        }
			//    }
			//}

			if (!isPasswordValid) {
				return View(userToEdit);
			}

			userToEdit.PasswordHash = passwordHasher.HashPassword(userToEdit, password);
			userToEdit.Email = email;
			IdentityResult saveResult = await userManager.UpdateAsync(userToEdit);

			if (saveResult.Succeeded) {
				return RedirectToAction("Index");
			}
			else {
				foreach (IdentityError? error in saveResult.Errors) {
					ModelState.AddModelError("", error.Description);
				}
				return View(userToEdit);
			}
		}
	}

	public async Task<IActionResult> Delete(string id) {
		AppUser userToDelete = await userManager.FindByIdAsync(id);
		if (userToDelete == null) {
			ModelState.AddModelError("", "User not found");
		}
		else {
			IdentityResult deleteResult = await userManager.DeleteAsync(userToDelete);
			if (!deleteResult.Succeeded) {
				ModelState.AddModelError("", "Delete operation failed");
			}
		}

		return RedirectToAction("Index");
	}
}
