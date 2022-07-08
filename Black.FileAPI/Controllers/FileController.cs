using Black.Service.FileService;
using DotNetCore.AspNetCore;
using DotNetCore.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Black.FileAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FileController : ControllerBase
{
    private readonly string _directory;
    private readonly IFileService _fileService;

    public FileController
    (
        IFileService fileService,
        IHostEnvironment environment
    )
    {
        _directory = Path.Combine(environment.ContentRootPath, "File");
        _fileService = fileService;
    }

    [DisableRequestSizeLimit]
    [HttpPost("/add")]
    public Task<IEnumerable<BinaryFile>> AddAsync() => _fileService.AddAsync(_directory, Request.Files());


    [HttpGet("{id}")]
    public IActionResult Get(Guid id) => _fileService.GetAsync(_directory, id).FileResult();

}