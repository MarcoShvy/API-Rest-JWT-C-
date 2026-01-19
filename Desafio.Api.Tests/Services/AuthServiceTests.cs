using System.IdentityModel.Tokens.Jwt;
using Desafio.Api.Dtos.Request;
using Desafio.Api.Services;
using Desafio.Api.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using System.Security.Claims;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        
        var jwtSettings = new JwtSettings
        {
            Key = "testecahvesecretanaooficialmontreal0123456",
            Issuer = "desafio-api",
            Audience = "desafio-api-client"
        };

        
        var mockOptions = new Mock<IOptions<JwtSettings>>();
        mockOptions
            .Setup(o => o.Value)
            .Returns(jwtSettings);

        _authService = new AuthService(mockOptions.Object);
    }

    [Fact]
    public void Authenticate_CredenciaisValidas_DeveRetornarToken()
    {
        var request = new AuthRequestDTO("admin", "teste01");

        
        var token = _authService.Authenticate(request);

        
        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal("desafio-api", jwt.Issuer);
        Assert.Contains(jwt.Claims, c =>
    c.Type == ClaimTypes.Name && c.Value == "admin");

    }

    [Fact]
    public void Authenticate_UsuarioInvalido_DeveLancarUnauthorizedAccessException()
    {
        
        var request = new AuthRequestDTO("errado", "teste01");

        
        Assert.Throws<UnauthorizedAccessException>(() =>
            _authService.Authenticate(request));
    }

    [Fact]
    public void Authenticate_SenhaInvalida_DeveLancarUnauthorizedAccessException()
    {
        
        var request = new AuthRequestDTO("admin", "123");
        
        Assert.Throws<UnauthorizedAccessException>(() =>
            _authService.Authenticate(request));
    }
}
