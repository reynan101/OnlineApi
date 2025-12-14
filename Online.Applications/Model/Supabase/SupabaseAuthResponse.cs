namespace Online.Applications.Model.Supabase
{

    public class SupabaseAuthResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public AuthData? Data { get; set; }

        public class AuthData
        {
            public User? User { get; set; }
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
        }

        public class User
        {
            public string? Id { get; set; }
            public string? Email { get; set; }
        }
    }
}
