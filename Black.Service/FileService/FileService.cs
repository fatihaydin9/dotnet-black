using DotNetCore.Objects;

namespace Black.Service.FileService;

public sealed class FileService : IFileService
{
    public Task<IEnumerable<BinaryFile>> AddAsync(string directory, IEnumerable<BinaryFile> files)
    {
        return files.SaveAsync(directory);
    }

    public Task<BinaryFile> GetAsync(string directory, Guid id)
    {
        return BinaryFile.ReadAsync(directory, id);
    }
}
