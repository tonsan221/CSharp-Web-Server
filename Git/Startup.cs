namespace CarShop
{
    using System.Threading.Tasks;
    using Git.Data;
    using Git.Services;
    using Microsoft.EntityFrameworkCore;
    using MyWebServer;
    using MyWebServer.Controllers;
    using MyWebServer.Results.Views;

    public class Startup
    {
        public static async Task Main()
            => await HttpServer
                .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<IUsersService, UserService>()
                    .Add<IRepositoryService, RepositoryService>()
                    .Add<ICommitService, CommitService>()
                    .Add<ApplicationDbContext>())
                .WithConfiguration<ApplicationDbContext>(context => context
                    .Database.Migrate())
                .Start();
    }
}
