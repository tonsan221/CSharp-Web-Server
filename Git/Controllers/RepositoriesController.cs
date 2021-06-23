using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoryService repositoryService;

        public RepositoriesController(IRepositoryService repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        [Authorize]
        public HttpResponse All()
        {
            var repositories = this.repositoryService.GetPublicRepositories();

            return this.View(repositories);
        }

        [Authorize]

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string repositoryType)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3 || name.Length > 10)
            {
                return this.Error("Repository name must be between 3 and 10 characters long.");
            }

            this.repositoryService.Create(name, repositoryType.ToLower(), this.User.Id);

            return this.Redirect("/Repositories/All");
        }
    }
}
