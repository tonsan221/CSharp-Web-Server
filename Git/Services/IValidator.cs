using Git.Models.Users;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IValidator
    {
        ICollection<string> ValidateUserRegistration(RegisterFormModel model);

        ICollection<string> ValidateRepository(string name, string repositoryType);

        ICollection<string> ValidateCommit(string description);
    }
}
