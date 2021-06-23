using Git.Data;
using Git.Models.Users;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Text.RegularExpressions;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public HttpResponse Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            var userId = this.usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid username or password.");
            }

            this.SignIn(userId);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Register()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect("/Repositories/All");
            }

            return this.View();
        }

        [HttpPost] 
        public HttpResponse Register(RegisterFormModel input)
        {
            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("Username already exists.");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("Email already exists.");
            }

            if (string.IsNullOrEmpty(input.Username) || 
                input.Username.Length < 5 ||
                input.Username.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters.");
            }

            if (!Regex.IsMatch(input.Email, DataConstants.UserEmailRegularExpression))
            {
                return this.Error("Email is not valid.");
            }

            if (string.IsNullOrEmpty(input.Password) || 
                input.Password.Length < 6 ||
                input.Password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters.");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords must match.");
            }

            this.usersService.CreateUser(input.Username, input.Email, input.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignIn(this.User.Id);

            return this.Redirect("/");
        }
    }
}
