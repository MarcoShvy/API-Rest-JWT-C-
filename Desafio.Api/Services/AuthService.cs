using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Desafio.Api.Dtos.Request;
using Desafio.Api.Interfaces;
using Desafio.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Desafio.Api.Services;

public class AuthService : IAuthService
{
    private const string User = "admin";
    private const string Password = "teste01";

    private readonly JwtSettings _jwt;

    public AuthService(IOptions<JwtSettings> jwtOptions)
    {
        _jwt = jwtOptions.Value;
    }

    public string Authenticate(AuthRequestDTO request)
    {
        if (request.Username != User || request.Password != Password)
            throw new UnauthorizedAccessException("Usuário ou senha inválidos");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Key));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
