using Desafio.Api.Dtos.Request;
using Desafio.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Api.Controllers;

[ApiController]
[Authorize]
[Route("files")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("read")]
    public async Task<IActionResult> Read(FileReadRequestDTO request, CancellationToken ct)
    {
        var result = await _fileService.ReadAsync(request.Path, ct);
        return Ok(result);
    }
}
