using Compass.Data.Data.Classes;
using Compass.Data.Data.Interfaces;

namespace Compass.API.Infrasructure.Repositories
{
    public class RepositoriesConfiguration
    {
        public static void Configuration(IServiceCollection services)
        {
            // Add IUserRepository
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
