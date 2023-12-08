using ASP.NetSchool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASP.NetSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NetSchool.Controllers;

//[Authorize(Roles = "Admin")]
public class RolesController : Controller {
	private readonly RoleManager<IdentityRole> roleManager;
	private readonly UserManager<AppUser> userManager;

	public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
		this.roleManager = roleManager;
		this.userManager = userManager;
	}

	public IActionResult Index() {
		return View(roleManager.Roles);
	}

	public IActionResult Create() {
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(string name) {
		if (ModelState.IsValid) {
			IdentityResult createResult = await roleManager.CreateAsync(new IdentityRole(name));

			if (createResult.Succeeded) {
				return RedirectToAction("Index");
			}
			else {
				foreach (IdentityError? error in createResult.Errors) {
					ModelState.AddModelError("", error.Description);
				}
			}
		}

		return View(name);
	}

	[HttpPost]
	public async Task<IActionResult> Delete(string id) {
		IdentityRole roleToDelete = await roleManager.FindByIdAsync(id + "asdasd");
		if (roleToDelete != null) {
			IdentityResult deleteResult = await roleManager.DeleteAsync(roleToDelete);

			if (deleteResult.Succeeded) {
				return RedirectToAction("Index");
			}
			else {
				foreach (IdentityError? error in deleteResult.Errors) {
					ModelState.AddModelError("", error.Description);
					return View();
				}
			}

			return View(roleToDelete.Name);
		}
		else {
			ModelState.AddModelError("", "Role not found!");
			return View();
		}

	}

	public async Task<IActionResult> Edit(string id) {
		IdentityRole role = await roleManager.FindByIdAsync(id);
		List<AppUser> members = new();
		List<AppUser> nonMembers = new();

		if (role != null) {
			foreach (var user in userManager.Users) {
				var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
				list.Add(user);
			}

			return View(new RoleEditViewModel {
				Members = members,
				NonMembers = nonMembers,
				Role = role
			});
		}

		return View("NotFound");
	}

	[HttpPost]
	public async Task<IActionResult> Edit(RoleModificationViewModel model) {
		IdentityResult result;
		if (ModelState.IsValid) {
			foreach (string userId in model.IdsToAdd ?? new string[] { }) {
				var user = await userManager.FindByIdAsync(userId);
				if (user != null) {
					result = await userManager.AddToRoleAsync(user, model.RoleName);
					if (!result.Succeeded) {
						foreach (var error in result.Errors) {
							ModelState.AddModelError("", error.Description);
						}
					}
				}
			}
			foreach (string userId in model.IdsToDelete ?? new string[] { }) {
				var user = await userManager.FindByIdAsync(userId);
				if (user != null) {
					result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
					if (!result.Succeeded) {
						foreach (var error in result.Errors) {
							ModelState.AddModelError("", error.Description);
						}
					}
				}
			}
			return RedirectToAction("Index");
		}
		else {
			return RedirectToAction("Edit", "Roles", model.RoleId);
		}
	}
}
