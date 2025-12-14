using Online.Applications.Model.Supabase;
using FluentResults;

namespace Online.Applications.Interface
{
    public interface ISupabaseService
    {
        Task<Result<SupabaseAuthResponse?>> AuthenticateAsync(AuthenticationRequest request);
    }
}
