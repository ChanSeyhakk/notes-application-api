using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NotesApp.API.Auth;
using NotesApp.API.Data;
using NotesApp.API.Repositories;
using NotesApp.API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ========================================
// 1. Controllers
// ========================================

builder.Services.AddControllers();


// ========================================
// 2. CORS
// ========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ========================================
// 3. JWT Authentication
// ========================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Validate token issuer
                ValidateIssuer = true,

                // Validate token audience
                ValidateAudience = true,

                // Validate token expiration
                ValidateLifetime = true,

                // Validate signing key
                ValidateIssuerSigningKey = true,

                // Issuer from appsettings.json
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                // Audience from appsettings.json
                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                // Secret key from appsettings.json
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });


// ========================================
// 4. Authorization
// ========================================

builder.Services.AddAuthorization();


// ========================================
// 5. Database
// ========================================

builder.Services.AddSingleton<DbConnectionFactory>();


// ========================================
// 6. Repositories
// ========================================

builder.Services.AddScoped<NoteRepository>();
builder.Services.AddScoped<UserRepository>();


// ========================================
// 7. Services
// ========================================

builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtTokenService>();


// ========================================
// 8. Swagger
// ========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ========================================
// Build Application
// ========================================

var app = builder.Build();


// ========================================
// 9. Swagger
// ========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ========================================
// 10. CORS
// ========================================

app.UseCors("VueClient");


// ========================================
// 11. Authentication
// ========================================

app.UseAuthentication();


// ========================================
// 12. Authorization
// ========================================

app.UseAuthorization();


// ========================================
// 13. Controllers
// ========================================

app.MapControllers();


// ========================================
// 14. Run Application
// ========================================

app.Run();