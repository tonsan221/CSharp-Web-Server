using Git.Models.Users;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Git.Data.DataConstants;


namespace Git.Services
{
    public class Validator : IValidator
    {
        private readonly IUsersService usersService;
        private readonly IRepositoryService repositoryService;
        private readonly ICommitService commitService;

        
        public Validator(IUsersService usersService, IRepositoryService repositoryService, ICommitService commitService)
        {
            this.usersService = usersService;
            this.repositoryService = repositoryService;
            this.commitService = commitService;
        }
        public ICollection<string> ValidateCommit(string description)
        {
            var errors = new List<string>();


            if (string.IsNullOrEmpty(description) || description.Length < 5)
            {
                errors.Add("Description must be more than 5 characters.");
            }

            return errors;
        }

        public ICollection<string> ValidateRepository(string name, string repositoryType)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(name) || name.Length < 3 || name.Length > 10)
            {
                errors.Add("Repository name must be between 3 and 10 characters long.");
            }

            if (repositoryType != "Private" && repositoryType != "Public")
            {
                errors.Add("Repositories are either public or private.");
            }

            return errors;

        }

        public ICollection<string> ValidateUserRegistration(RegisterFormModel model)
        {
            var errors = new List<string>();

            if (!this.usersService.IsUsernameAvailable(model.Username))
            {
                errors.Add("Username already exists.");
            }

            if (!this.usersService.IsEmailAvailable(model.Email))
            {
                errors.Add("Email already exists.");
            }

            if (string.IsNullOrEmpty(model.Username) ||
                model.Username.Length < 5 ||
                model.Username.Length > 20)
            {
                errors.Add("Username should be between 5 and 20 characters.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add("Email is not valid.");
            }

            if (string.IsNullOrEmpty(model.Password) ||
                model.Password.Length < 6 ||
                model.Password.Length > 20)
            {
                errors.Add("Password should be between 6 and 20 characters.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add("Passwords must match.");
            }

            return errors;
        }
    }
}
