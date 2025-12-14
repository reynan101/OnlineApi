using Microsoft.Extensions.DependencyInjection;
using Online.Applications.Interface;
using Online.Applications.Services;
namespace Online.Applications.Configurations
{
    public static class RegisterApplications
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISupabaseService, SupabaseServices>();
            return services;
        }
    }
}
