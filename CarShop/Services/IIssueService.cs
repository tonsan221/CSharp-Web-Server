using CarShop.Models.Issues;

namespace CarShop.Services
{
    public interface IIssueService
    {
        
        void Add(string description, string carId);
        string FixIssue(string issueId);
        string DeleteIssue(string issueId);
        bool UserOwnsCar(string carId, string userId);
        CarIssuesViewModel GetCarWithIssues(string carId);
    }
}
