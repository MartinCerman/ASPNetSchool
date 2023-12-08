using ASP.NetSchool.Models;
using ASP.NetSchool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetSchool.Controllers;

[Authorize(Roles = "Admin, Teacher, Student")]
public class SubjectsController : Controller {
	private SubjectService service;

	public SubjectsController(SubjectService service) {
		this.service = service;
	}

	public async Task<IActionResult> IndexAsync() {
		var allSubjects = await service.GetAllAsync();
		return View(allSubjects);
	}

	[Authorize(Roles = "Admin, Teacher")]
	public IActionResult Create() {
		return View();
	}

	[Authorize(Roles = "Admin, Teacher")]
	[HttpPost]
	public async Task<IActionResult> CreateAsync(Subject subject) {
		await service.CreateSubjectAsync(subject);
		return RedirectToAction("Index");
	}

	[Authorize(Roles = "Admin, Teacher")]
	public async Task<IActionResult> Edit(int id) {
		Subject? subject = await service.GetByIdAsync(id);
		if (subject == null) {
			return View("NotFound");
		}
		return View(subject);
	}

	[Authorize(Roles = "Admin, Teacher")]
	[HttpPost]
	public async Task<IActionResult> Edit([Bind("Id, Name")] Subject subject) {
		await service.UpdateAsync(subject);
		return RedirectToAction("Index");
	}

	[Authorize(Roles = "Admin, Teacher")]
	public async Task<IActionResult> Delete(int id) {
		Subject? subject = await service.GetByIdAsync(id);
		if (subject == null) {
			return View("NotFound");
		}
		await service.DeleteAsync(subject);
		return RedirectToAction("Index");
	}

}
