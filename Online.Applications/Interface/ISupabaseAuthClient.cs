using FluentResults;
using Online.Applications.Model.Supabase;

namespace Online.Applications.Interface
{
    public interface ISupabaseAuthClient
    {
        Task<Result<SupabaseAuthResponse?>> AuthenticateAsync(AuthenticationRequest request);
    }
}
