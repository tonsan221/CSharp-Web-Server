using Git.Data;
using Git.Data.Models;
using Git.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly ApplicationDbContext db;

        public RepositoryService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public void Create(string name, string type, string userId)
        {
            var repository = new Repository
            {
                Name = name,
                IsPublic = type == "private" ? false : true,
                CreatedOn = DateTime.UtcNow,
                OwnerId = userId
            };

            this.db.Repositories.Add(repository);
            this.db.SaveChanges();
        }

        public IEnumerable<RepositoryViewModel> GetPublicRepositories()
        {
            var repositories = this.db.Repositories
                .Where(x => x.IsPublic == true)
                .Select(x => new RepositoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedOn = x.CreatedOn.ToString("g"),
                    Owner = x.Owner.Username,
                    CommitsCount = x.Commits.Count
                }).ToList();

            return repositories;
        }
    }
}
