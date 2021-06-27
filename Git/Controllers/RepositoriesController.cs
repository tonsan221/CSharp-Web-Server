using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoryService repositoryService;
        private readonly IValidator validator;

        public RepositoriesController(IRepositoryService repositoryService, IValidator validator)
        {
            this.repositoryService = repositoryService;
            this.validator = validator;
        }

        public HttpResponse All()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Error("You must be logged in to access this resource.");
            }

            var repositories = this.repositoryService.GetPublicRepositories();

            return this.View(repositories);
        }


        public HttpResponse Create()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Error("You must be logged in to access this resource.");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string repositoryType)
        {
            var errors = this.validator.ValidateRepository(name, repositoryType);

            if (errors.Any())
            {
                return this.Error(errors);
            }

            this.repositoryService.Create(name, repositoryType.ToLower(), this.User.Id);

            return this.Redirect("/Repositories/All");
        }
    }
}
