using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Online.Applications.Interface;
using Online.Applications.Model.Configuration;
using Online.Infrastructure.Client.Supabase;

namespace Online.Infrastructure.Configurations
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind configuration
            services.Configure<SupabaseConfig>(configuration.GetSection("SupabaseConfig"));

            // Register Supabase client with HttpClient injection
            services.AddHttpClient<ISupabaseAuthClient, SupabaseAuthClient>();

            // Register RF bulk payments client with HttpClient injection
            services.AddHttpClient<IRFBulkPayments, RFBulkPayments>();

            return services;
        }
    }
}
