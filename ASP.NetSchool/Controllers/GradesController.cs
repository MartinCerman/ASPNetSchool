using ASP.NetSchool.Models;
using ASP.NetSchool.Services;
using ASP.NetSchool.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP.NetSchool.Controllers;

[Authorize(Roles = "Admin, Teacher")]
public class GradesController : Controller {
	private GradeService service;

	public GradesController(GradeService service) {
		this.service = service;
	}

	public async Task<IActionResult> Index() {
		var allGrades = await service.GetAllGrades();
		return View(allGrades);
	}

	[HttpGet]
	public async Task<IActionResult> Create() {
		var gradesDropdownData = await service.GetDropdownValuesAsync();
		ViewBag.Students = new SelectList(gradesDropdownData.Students, "Id", "LastName");
		ViewBag.Subjects = new SelectList(gradesDropdownData.Subjects, "Id", "Name");
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(GradeViewModel gradeViewModel) {
		await service.CreateAsync(gradeViewModel);
		return RedirectToAction("Index");
	}
	[HttpGet]
	public async Task<IActionResult> Edit(int id) {
		var gradesDropdownData = await service.GetDropdownValuesAsync();
		ViewBag.Students = new SelectList(gradesDropdownData.Students, "Id", "LastName");
		ViewBag.Subjects = new SelectList(gradesDropdownData.Subjects, "Id", "Name");
		GradeViewModel? gradeToEdit = service.GetById(id);
		if(gradeToEdit is null) {
			return View("NotFound");
		}
		return View(gradeToEdit);
	}
	[HttpPost]
	public async Task<IActionResult> Edit(int id, GradeViewModel updatedGrade) {
		updatedGrade.Id = id;
		await service.Update(updatedGrade);
		return RedirectToAction("Index");
	}
	[HttpPost]
	public async Task<IActionResult> Delete(int id) {
		await service.DeleteAsync(id);
		return RedirectToAction("Index");
	}
}
