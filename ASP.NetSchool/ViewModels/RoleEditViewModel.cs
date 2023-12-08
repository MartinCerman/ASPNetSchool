using ASP.NetSchool.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP.NetSchool.ViewModels {
	public class RoleEditViewModel {
		public IdentityRole Role { get; set; } = null!;
		public IEnumerable<AppUser> Members { get; set; } = null!;
		public IEnumerable<AppUser> NonMembers { get; set; } = null!;
	}
}
