namespace CarShop.Controllers
{
    using CarShop.Services;
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly IIssueService issueService;

        public IssuesController(
            IUserService userService, 
            IIssueService issueService)
        {
            this.userService = userService;
            this.issueService = issueService;
        }

        public HttpResponse Add()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Add(string description, string carId)
        {
            if (string.IsNullOrEmpty(description) || description.Length < 5)
            {
                return Error("Description should be filled and must be more than 5 characters.");
            }

            this.issueService.Add(description, carId);
            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse Fix(string issueId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                return Error("Clients cannot fix cars.");
            }

            var carId = this.issueService.FixIssue(issueId);
            return Redirect($"/Issues/CarIssues?carId={carId}");
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnsCar = this.issueService.UserOwnsCar(carId, this.User.Id);

                if (!userOwnsCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.issueService.GetCarWithIssues(carId);

            if (carWithIssues == null)
            {
                return Error($"Car with ID '{carId}' does not exist.");
            }

            return View(carWithIssues);
        }
    }
}
