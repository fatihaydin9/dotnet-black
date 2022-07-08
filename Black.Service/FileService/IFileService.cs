using DotNetCore.Objects;

namespace Black.Service.FileService;

public interface IFileService
{
    Task<IEnumerable<BinaryFile>> AddAsync(string directory, IEnumerable<BinaryFile> files);
    Task<BinaryFile> GetAsync(string directory, Guid id);
}

