using Compass.Data.Data.AutoMapper;

namespace Compass.API.Infrasructure.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void Configuration(IServiceCollection services)
        {
            //Add AutoMapper User
            services.AddAutoMapper(typeof(AutoMapperUserProfile));
        }
    }
}
