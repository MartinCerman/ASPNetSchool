using ASP.NetSchool.Models;
using ASP.NetSchool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml;

namespace ASP.NetSchool.Controllers {
	public class FileUploadController : Controller {
		StudentService studentService;

		public FileUploadController(StudentService studentService) {
			this.studentService = studentService;
		}

		public async Task<IActionResult> UploadAsync(IFormFile file) {
			if (file.Length > 0) {
				string filePath = Path.GetFullPath(file.FileName);

				using (FileStream stream = new(filePath, FileMode.Create)) {
					await file.CopyToAsync(stream);
				}

				XmlDocument xmlDocument = new();
				xmlDocument.Load(filePath);
				XmlElement? rootNode = xmlDocument.DocumentElement;
				if (rootNode != null) {
					foreach (XmlNode node in rootNode.SelectNodes("/Students/Student")) {
						Student newStudent = new() {
							FirstName = node.ChildNodes[0].InnerText,
							LastName = node.ChildNodes[1].InnerText,
							DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ"))
						};
						await studentService.CreateStudentAsync(newStudent);
					}
				}
				return RedirectToAction("Index", "Students");
			}

			return View("NotFound");
		}
	}
}
