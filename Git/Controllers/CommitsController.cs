using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ICommitService commitService;

        public CommitsController(ICommitService commitService)
        {
            this.commitService = commitService;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = this.commitService.GetRepositoryCommits(id);

            return this.View(repository);
        }


        [HttpPost]
        public HttpResponse Create(string description, string id)
        {
            if (string.IsNullOrEmpty(description) || description.Length < 5)
            {
                return this.Error("Description must be more than 5 characters.");
            }

            this.commitService.Create(description, id, this.User.Id);

            return this.Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var userCommits = this.commitService.GetAllUserCommits(this.User.Id);

            return this.View(userCommits);
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            this.commitService.Delete(id);
;
            return this.Redirect("/Commits/All");
        }
    }
}
