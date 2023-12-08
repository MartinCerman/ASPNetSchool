using ASP.NetSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NetSchool.Services {
	public class SubjectService {
		ApplicationDbContext dbContext;

		public SubjectService(ApplicationDbContext dbContext) {
			this.dbContext = dbContext;
		}

        internal async Task CreateSubjectAsync(Subject subject) {
            await dbContext.AddAsync(subject);
            await dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(Subject subject) {
            dbContext.Subjects.Remove(subject);
            await dbContext.SaveChangesAsync();
        }

        internal async Task<IEnumerable<Subject>> GetAllAsync() {
            return await dbContext.Subjects.ToListAsync();
        }

        internal async Task<Subject?> GetByIdAsync(int id) {
            return await dbContext.Subjects.FirstOrDefaultAsync(x => x.Id == id);
        }

        internal async Task UpdateAsync(Subject subject) {
            dbContext.Subjects.Update(subject);
            await dbContext.SaveChangesAsync();
        }
    }
}
