using Online.Applications.Interface;
using Online.Applications.Model.Supabase;
using FluentResults;

namespace Online.Applications.Services
{
    public class SupabaseServices : ISupabaseService
    {
        private readonly ISupabaseAuthClient _supabaseAuthClient;

        public SupabaseServices(ISupabaseAuthClient supabaseAuthClient)
        {
            _supabaseAuthClient = supabaseAuthClient;
        }
        public async Task<Result<SupabaseAuthResponse?>> AuthenticateAsync(AuthenticationRequest request)
        {
           return await _supabaseAuthClient.AuthenticateAsync(request);
        }
    }
}
