using ASP.NetSchool.Models;
using ASP.NetSchool.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetSchool.Controllers {
	public class StudentsController : Controller {
		private StudentService service;

		public StudentsController(StudentService service) {
			this.service = service;
		}

		public async Task<IActionResult> IndexAsync() {
			var allStudents = await service.GetAllAsync();
			return View(allStudents);
		}

		public IActionResult Create() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(Student student) {
			await service.CreateStudentAsync(student);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id) {
			Student? student = await service.GetByIdAsync(id);
			if (student == null) {
				return View("NotFound");
			}
			return View(student);
		}

		[HttpPost]
		public async Task<IActionResult> Edit([Bind("Id, FirstName, LastName, DateOfBirth")] Student student) {
			await service.UpdateAsync(student);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id) {
			Student? student = await service.GetByIdAsync(id);
			if(student == null) {
				return View("NotFound");
			}
			await service.DeleteAsync(student);
			return RedirectToAction("Index");
		}
	}
}

