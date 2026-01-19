using Desafio.Api.Dtos.Response;

namespace Desafio.Api.Interfaces;

public interface IFileService
{
    Task<FileReadResponseDTO> ReadAsync(string path, CancellationToken ct);
}
