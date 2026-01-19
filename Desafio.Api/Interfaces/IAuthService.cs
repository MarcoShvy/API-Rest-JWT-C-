using Desafio.Api.Dtos.Request;

namespace Desafio.Api.Interfaces;

public interface IAuthService
{
    string Authenticate(AuthRequestDTO request);
}
