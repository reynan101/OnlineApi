using Microsoft.Extensions.Options;
using Online.Applications.Model.Configuration;

namespace Online.Infrastructure.Client.Supabase
{
    public abstract class BaseSupabaseClient
    {
        private readonly HttpClient _httpClient;
        protected readonly SupabaseConfig _supabaseConfig;

        protected BaseSupabaseClient(HttpClient httpClient, IOptions<SupabaseConfig> supabaseConfig)
        {
            _httpClient = httpClient;
            _supabaseConfig = supabaseConfig.Value;
            SetDefaultHeaders();
        }

        protected HttpClient Client => _httpClient;

        private void SetDefaultHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _supabaseConfig.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_supabaseConfig.AnonKey}");
        }
    }

}
