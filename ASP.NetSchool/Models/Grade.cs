namespace ASP.NetSchool.Models {
	public class Grade {
		public int Id { get; set; }
		public int Mark { get; set; }
		public string Topic { get; set; } = null!;
		public DateTime Date { get; set; }
		public virtual Subject Subject { get; set; } = null!;
		public virtual Student Student { get; set; } = null!;
	}
}
