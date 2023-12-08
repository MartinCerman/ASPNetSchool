namespace ASP.NetSchool.ViewModels {
    public class LoginViewModel {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ReturnUrl { get; set; }
        public bool Remember {  get; set; }

    }
}
