using Git.Models.Repositories;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IRepositoryService
    {
        void Create(string name, string type, string userId);
        IEnumerable<RepositoryViewModel> GetPublicRepositories();
    }
}
