using Desafio.Api.Dtos.Response;
using Desafio.Api.Interfaces;

namespace Desafio.Api.Services;

public class FileService : IFileService
{
    private readonly string _basePath;

    public FileService(IConfiguration configuration)
    {
        _basePath = configuration["FileSettings:BasePath"]
            ?? throw new InvalidOperationException("BasePath não configurado");
    }

    public async Task<FileReadResponseDTO> ReadAsync(string path, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Caminho inválido");

            var fullPath = Path.GetFullPath(Path.Combine(_basePath, path));

            if (!fullPath.StartsWith(_basePath, StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Acesso não permitido");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Arquivo não encontrado");

            var content = await File.ReadAllTextAsync(fullPath, ct);

            return new FileReadResponseDTO(
                Path.GetFileName(fullPath),
                content
            );
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (IOException)
        {
            throw new IOException("Erro ao ler o arquivo!");
        }
        catch (Exception)
        {
            throw new Exception("Erro inesperado ao processar o arquivo");
        }
    }
}
