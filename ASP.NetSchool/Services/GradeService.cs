using ASP.NetSchool.Models;
using ASP.NetSchool.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ASP.NetSchool.Services {
	public class GradeService {
		private ApplicationDbContext dbContext;

		public GradeService(ApplicationDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public async Task<GradesDropdownsViewModel> GetDropdownValuesAsync() {
			return new GradesDropdownsViewModel() {
				Students = await dbContext.Students.OrderBy(x => x.LastName).ToListAsync(),
				Subjects = await dbContext.Subjects.OrderBy(x => x.Name).ToListAsync(),
			};
		}

		internal async Task CreateAsync(GradeViewModel gradeViewModel) {
			var gradeToInsert = new Grade() {
				Student = dbContext.Students.FirstOrDefault(s => s.Id == gradeViewModel.StudentId),
				Subject = dbContext.Subjects.FirstOrDefault(s => s.Id == gradeViewModel.SubjectId),
				Date = DateTime.Today,
				Topic = gradeViewModel.Topic,
				Mark = gradeViewModel.Mark
			};

			if (gradeToInsert.Student is not null && gradeToInsert.Subject is not null) {
				await dbContext.Grades.AddAsync(gradeToInsert);
				await dbContext.SaveChangesAsync();
			}
		}

		internal async Task<IEnumerable<Grade>> GetAllGrades() {
			return await dbContext.Grades.Include(g => g.Student).Include(g => g.Subject).ToListAsync();
		}

		internal GradeViewModel? GetById(int id) {
			Grade? gradeToEdit = dbContext.Grades.FirstOrDefault(g => g.Id == id);
			GradeViewModel gradeViewModel = new();
			if (gradeToEdit != null) {
				gradeViewModel.SubjectId = gradeToEdit.Subject.Id;
				gradeViewModel.StudentId = gradeToEdit.Student.Id;
				gradeViewModel.Id = gradeToEdit.Id;
				gradeViewModel.Mark = gradeToEdit.Mark;
				gradeViewModel.Date = gradeToEdit.Date;
				gradeViewModel.Topic = gradeToEdit.Topic;
			} else {
				return null;
			}
			return gradeViewModel;
		}

		internal async Task Update(GradeViewModel updatedGrade) {
			Grade? gradeToUpdate = dbContext.Grades.FirstOrDefault(g => g.Id == updatedGrade.Id);

			if (gradeToUpdate != null) {
				gradeToUpdate.Subject = dbContext.Subjects.FirstOrDefault(s => s.Id == updatedGrade.SubjectId)!;
				gradeToUpdate.Student = dbContext.Students.FirstOrDefault(s => s.Id == updatedGrade.StudentId)!;
				gradeToUpdate.Topic = updatedGrade.Topic;
				gradeToUpdate.Mark = updatedGrade.Mark;
				gradeToUpdate.Date = DateTime.Now;
			}

			dbContext.Update(gradeToUpdate);
			await dbContext.SaveChangesAsync();
		}

		internal async Task DeleteAsync(int id) {
			Grade? gradeToDelete = dbContext.Grades.FirstOrDefault(g=>g.Id == id);
			if (gradeToDelete != null) {
				dbContext.Remove(gradeToDelete);
				await dbContext.SaveChangesAsync();
			}
		}
	}
}
