using ASP.NetSchool.Models;

namespace ASP.NetSchool.ViewModels {
	public class GradeViewModel {
		public int Id { get; set; }
		public int Mark { get; set; }
		public string Topic { get; set; } = null!;
		public DateTime Date { get; set; }
		public int SubjectId { get; set; }
		public int StudentId { get; set; }
	}
}
