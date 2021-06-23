using Git.Models.Commits;
using Git.Models.Repositories;
using System.Collections.Generic;

namespace Git.Services
{
    public interface ICommitService
    {
        IEnumerable<CommitsViewModel> GetAllUserCommits(string userId);
        RepositoryViewModel GetRepositoryCommits(string repositoryId);
        void Create(string description, string repositoryId, string creatorId);
        void Delete(string commitId);
    }
}
