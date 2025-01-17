using System.Text;
using BE_lab2.Data;
using BE_lab2.Initializer;
using BE_lab2.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string? getEnv = Environment.GetEnvironmentVariable("DefaultConnection") ?? builder.Configuration.GetConnectionString("DefaultConnection");
string? issuer = Environment.GetEnvironmentVariable("Issuer") ?? builder.Configuration.GetSection("JWT")["Issuer"];
string? audience = Environment.GetEnvironmentVariable("Audience") ?? builder.Configuration.GetSection("JWT")["Audience"];
string? key = Environment.GetEnvironmentVariable("Key") ?? builder.Configuration.GetSection("JWT")["Key"];
string? tokenValidityMins = Environment.GetEnvironmentVariable("TokenValidityMins") ?? builder.Configuration.GetSection("JWT")["TokenValidityMins"];

builder.Services.Configure<JwtOptions>(options =>
{
    options.Issuer = issuer;
    options.Audience = audience;
    options.Key = key;
    options.TokenValidityMins = tokenValidityMins;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(getEnv));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true

    };
});
builder.Services.AddAuthorization();
builder.Services.AddScoped<JWTService>();
builder.Services.AddScoped<IDbInitializer, DbInitialize>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT access token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {jwtSecurityScheme, Array.Empty<string>()}
    });
});
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();