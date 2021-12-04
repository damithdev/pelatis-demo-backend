using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pelatis.Data;
using Pelatis.Data.Repositories;
using Pelatis.Services;

namespace Pelatis.Config.Extensions
{
    public static class ApplicationServiceExtenstions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));

            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultDBConnection")));

            // Repository
            services.AddScoped<IAppUserRepository, AppUserRepositoryImpl>();
            services.AddScoped<IBusinessRepository, BusinessRepositoryImpl>();
            services.AddScoped<ICustomerRepository, CustomerRepositoryImpl>();

            // Services
            services.AddScoped<ITokenService, TokenService>();



            return services;
        }
    }
}
