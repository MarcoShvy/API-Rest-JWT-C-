using System.Text;
using Desafio.Api.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class FileServiceTests
{
    private readonly string _basePath;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        _basePath = Path.Combine(Path.GetTempPath(), "files-test");
        Directory.CreateDirectory(_basePath);

        
        var mockConfig = new Mock<IConfiguration>();

        mockConfig
            .Setup(c => c["FileSettings:BasePath"])
            .Returns(_basePath);

        _fileService = new FileService(mockConfig.Object);
    }

    [Fact]
    public async Task ReadAsync_DeveRetornarConteudoDoArquivo()
    {
        
        var fileName = "teste.txt";
        var filePath = Path.Combine(_basePath, fileName);
        var content = "conteudo de teste";

        await File.WriteAllTextAsync(filePath, content);

       
        var result = await _fileService.ReadAsync(fileName, CancellationToken.None);

        
        Assert.Equal(fileName, result.FileName);
        Assert.Equal(content, result.Content);
    }

    [Fact]
    public async Task ReadAsync_CaminhoVazio_DeveLancarArgumentException()
    {
        
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _fileService.ReadAsync("", CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_ArquivoInexistente_DeveLancarFileNotFoundException()
    {
        
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            _fileService.ReadAsync("nao-existe.txt", CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_PathTraversal_DeveLancarUnauthorizedAccessException()
    {
        
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _fileService.ReadAsync("../segredo.txt", CancellationToken.None));
    }
}
