using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Online.Applications.Configurations;
using Online.Applications.Model.Configuration;
using Online.Infrastructure.Configurations;
using System.Security.Cryptography;

// ----------------------------
// Build app
// ----------------------------
var builder = WebApplication.CreateBuilder(args);

// Add infrastructure & application services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Bind SupabaseConfig
var supabaseConfig = builder.Configuration
    .GetSection("SupabaseConfig")
    .Get<SupabaseConfig>();

if (supabaseConfig == null || string.IsNullOrEmpty(supabaseConfig.Url))
    throw new Exception("SupabaseConfig is not properly configured!");

// ----------------------------
// Add controllers
// ----------------------------
builder.Services.AddControllers();

// ----------------------------
// Cache for JWKS keys
// ----------------------------
var jwksCache = new Dictionary<string, SecurityKey>();

// ----------------------------
// JWT Authentication (ES256 with cached JWKS)
// ----------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true; // false only for local dev
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{supabaseConfig.Url}/auth/v1",

            ValidateAudience = true,
            ValidAudience = "authenticated",

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            // Use JWKS with caching
            IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
            {
                if (jwksCache.TryGetValue(kid, out var cachedKey))
                    return new[] { cachedKey };

                using var http = new HttpClient();
                var jwksJson = http.GetStringAsync($"{supabaseConfig.Url}/auth/v1/.well-known/jwks.json").Result;
                var jwks = JObject.Parse(jwksJson)["keys"];
                var keys = new List<SecurityKey>();

                foreach (var key in jwks)
                {
                    if (key["kid"]?.ToString() == kid)
                    {
                        var ecdsa = ECDsa.Create();
                        var x = Base64UrlEncoder.DecodeBytes(key["x"].ToString());
                        var y = Base64UrlEncoder.DecodeBytes(key["y"].ToString());
                        var ecParams = new ECParameters
                        {
                            Curve = ECCurve.NamedCurves.nistP256,
                            Q = new ECPoint { X = x, Y = y }
                        };
                        ecdsa.ImportParameters(ecParams);
                        var securityKey = new ECDsaSecurityKey(ecdsa) { KeyId = key["kid"].ToString() };
                        jwksCache[kid] = securityKey;
                        keys.Add(securityKey);
                    }
                }

                return keys;
            }
        };
    });

// ----------------------------
// Authorization
// ----------------------------
builder.Services.AddAuthorization();

// ----------------------------
// Swagger with JWT support
// ----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online.Api", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ----------------------------
// Middleware
// ----------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Online.Api");
        c.RoutePrefix = "";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ✅ must come before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
