namespace ChaychiMenu.Infrastructure.ServiceContracts;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string contentType, CancellationToken ct = default);
    Task DeleteAsync(string fileUrl, CancellationToken ct = default);
}