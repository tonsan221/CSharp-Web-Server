using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ICommitService commitService;
        private readonly IValidator validator;

        public CommitsController(ICommitService commitService, IValidator validator)
        {
            this.commitService = commitService;
            this.validator = validator;
        }

        public HttpResponse Create(string id)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Error("You must be logged in to access this resource.");
            }

            var repository = this.commitService.GetRepositoryCommits(id);

            return this.View(repository);
        }


        [HttpPost]
        public HttpResponse Create(string description, string id)
        {
            var errors = this.validator.ValidateCommit(description);

            if (errors.Any())
            {
                return this.Error(errors);
            }

            this.commitService.Create(description, id, this.User.Id);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse All()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Error("You must be logged in to access this resource.");
            }

            var userCommits = this.commitService.GetAllUserCommits(this.User.Id);

            return this.View(userCommits);
        }

        public HttpResponse Delete(string id)
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Error("You must be logged in to access this resource.");
            }

            this.commitService.Delete(id);
;
            return this.Redirect("/Commits/All");
        }
    }
}
