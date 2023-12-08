using ASP.NetSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NetSchool.Services {
	public class StudentService {
		private ApplicationDbContext dbContext;

		public StudentService(ApplicationDbContext dbContext) {
			this.dbContext = dbContext;
		}

		public async Task<IEnumerable<Student>> GetAllAsync() {
			return await dbContext.Students.ToListAsync();
		}

		public async Task<Student[]> GetAllArrayAsync() {
			return await dbContext.Students.ToArrayAsync();
		}

		public async Task CreateStudentAsync(Student student) {
			await dbContext.Students.AddAsync(student);
			await dbContext.SaveChangesAsync();
		}

		internal async Task<Student?> GetByIdAsync(int id) {
			return await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
		}

		internal async Task UpdateAsync(Student student) {
			dbContext.Students.Update(student);
			await dbContext.SaveChangesAsync();
		}

		internal async Task DeleteAsync(Student student) {
			dbContext.Students.Remove(student);
			await dbContext.SaveChangesAsync();
		}
	}
}
