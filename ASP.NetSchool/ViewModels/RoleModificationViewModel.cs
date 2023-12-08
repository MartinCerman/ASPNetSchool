namespace ASP.NetSchool.ViewModels {
	public class RoleModificationViewModel {
		public string RoleName { get; set; } = null!;
		public string RoleId { get; set; } = null!;
		public string[]? IdsToAdd { get; set; }
		public string[]? IdsToDelete { get; set; }
	}
}
