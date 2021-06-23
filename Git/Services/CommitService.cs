using Git.Data;
using Git.Data.Models;
using Git.Models.Commits;
using Git.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class CommitService : ICommitService
    {
        private readonly ApplicationDbContext db;

        public CommitService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(string description, string repositoryId, string creatorId)
        {
            var commit = new Commit
            {
                Description = description,
                CreatedOn = DateTime.UtcNow,
                RepositoryId = repositoryId,
                CreatorId = creatorId
            };

            this.db.Commits.Add(commit);
            this.db.SaveChanges();
        }

        public RepositoryViewModel GetRepositoryCommits(string repositoryId)
            => this.db.Repositories
                    .Where(x => x.Id == repositoryId)
                    .Select(x => new RepositoryViewModel
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                   .FirstOrDefault();

        public IEnumerable<CommitsViewModel> GetAllUserCommits(string userId)
            => this.db.Commits
                    .Where(x => x.CreatorId == userId)
                    .Select(x => new CommitsViewModel
                    {
                        Id = x.Id,
                        RepositoryName = x.Repository.Name,
                        Description = x.Description,
                        CreatedOn = x.CreatedOn.ToString("g"),
                    }).ToList();

        public void Delete(string commitId)
        {
            var commit = this.db.Commits
                    .FirstOrDefault(x => x.Id == commitId);

            this.db.Commits.Remove(commit);
            this.db.SaveChanges();
        }
    }
}
