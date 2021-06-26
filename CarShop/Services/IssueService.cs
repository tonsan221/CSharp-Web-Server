namespace CarShop.Services
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.Models.Issues;
    using System.Linq;

    public class IssueService : IIssueService
    {
        private readonly CarShopDbContext db;

        public IssueService(CarShopDbContext db)
        {
            this.db = db;
        }
        public void Add(string description, string carId)
        {
            var issue = new Issue
            {
                Description = description,
                IsFixed = false,
                CarId = carId
            };

            this.db.Issues.Add(issue);
            this.db.SaveChanges();
        }

        public string DeleteIssue(string issueId)
        {
            var issue = this.db.Issues.FirstOrDefault(x => x.Id == issueId);

            this.db.Issues.Remove(issue);
            this.db.SaveChanges();

            return issue.CarId;
        }

        public string FixIssue(string issueId)
        {
            var issue = this.db.Issues.FirstOrDefault(x => x.Id == issueId);
            issue.IsFixed = true;

            this.db.Issues.Update(issue);
            this.db.SaveChanges();

            return issue.CarId;
        }

        public CarIssuesViewModel GetCarWithIssues(string carId)
        {
            return this.db
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        Description = i.Description,
                        IsFixed = i.IsFixed
                    })
                })
                .FirstOrDefault();
        }

        public bool UserOwnsCar(string carId, string userId) => this.db.Cars
                    .Any(c => c.Id == carId && c.OwnerId == userId);
    }
}
