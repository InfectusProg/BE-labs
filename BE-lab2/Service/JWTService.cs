namespace BE_lab2.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BE_lab2.Data;
using BE_lab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using BE_lab2.Contracts;

public class JWTService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public JWTService(AppDbContext db, IOptions<JwtOptions> jwtOptions)
    {
        _db = db;
        _passwordHasher = new PasswordHasher<User>();
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResponse?> Authenticate(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var userAccount = await _db.Users.FirstOrDefaultAsync(u => u.Name == request.UserName);
        if (userAccount == null || string.IsNullOrWhiteSpace(userAccount.Password))
            return null;

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(userAccount, userAccount.Password, request.Password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
            return null;

        var issuer = _jwtOptions.Issuer;
        var audience = _jwtOptions.Audience;
        var key = _jwtOptions.Key;
        var tokenMin = Convert.ToInt32(_jwtOptions.TokenValidityMins);
        var tokenExpiryTimeStap = DateTime.UtcNow.AddMinutes(tokenMin);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                //new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            }),
            Expires = tokenExpiryTimeStap,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            UserName = request.UserName,
            ExpiresIn = (int)tokenExpiryTimeStap.Subtract(DateTime.UtcNow).TotalSeconds
        };
    }

    public async Task<string?> AddUserWithCurrencyAsync(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
            return "User name cannot be empty.";

        var existingUser = _db.Users.FirstOrDefault(u => u.Name == request.UserName);
        if (existingUser != null)
            return "A user with this name already exists.";

        var currency = await GetCurrencyAsync(request.CurrencyId);
        if (currency == null)
            return "Invalid currency ID or default currency not available.";

        var newUser = new User
        {
            Name = request.UserName,
            CurrencyId = currency.Id,
            Password = _passwordHasher.HashPassword(null, request.Password)
        };

        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();

        return null;
    }

    private async Task<Currency?> GetCurrencyAsync(int? currencyId)
    {
        if (currencyId == 0)
            return await _db.Currencies.FirstOrDefaultAsync(c => c.Name == "USD");

        return await _db.Currencies.FindAsync(currencyId);
    }
}