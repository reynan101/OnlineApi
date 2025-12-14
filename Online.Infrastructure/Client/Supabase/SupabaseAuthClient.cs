using FluentResults;
using Microsoft.Extensions.Options;
using Online.Applications.Interface;
using Online.Applications.Model.Configuration;
using Online.Applications.Model.Supabase;
using System.Text;
using System.Text.Json;

namespace Online.Infrastructure.Client.Supabase
{
    public class SupabaseAuthClient : BaseSupabaseClient, ISupabaseAuthClient
    {
        public SupabaseAuthClient(HttpClient httpClient, 
            IOptions<SupabaseConfig> supabaseConfig) : base(httpClient, supabaseConfig)
        {
        }

        public async Task<Result<SupabaseAuthResponse?>> AuthenticateAsync(AuthenticationRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                    return Result.Fail<SupabaseAuthResponse?>("Email and password are required.");

                // Serialize the request to JSON
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_supabaseConfig.Url}/functions/v1/user_authentication";
                var response = await Client.PostAsync(url, content);

                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Fail<SupabaseAuthResponse?>(
                        $"Failed to call authenticate function: {response.StatusCode} - {body}"
                    );
                }

                var result = JsonSerializer.Deserialize<SupabaseAuthResponse>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<SupabaseAuthResponse?>(
                    $"Exception while calling authenticate function: {ex.Message}"
                );
            }
        }

    }
}
