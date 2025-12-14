using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online.Applications.Interface;
using Online.Applications.Model.Supabase;

namespace Online.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISupabaseService _supabaseService;

        public AccountController(ISupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest request)
        {
            var result = await _supabaseService.AuthenticateAsync(request);

            var response = new SupabaseAuthResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.IsSuccess ? "Login successful" : string.Join("; ", result.Errors.Select(e => e.Message)),
                Data = result.IsSuccess ? result.Value?.Data : null
            };

            return Ok(response);
        }

    }
}
