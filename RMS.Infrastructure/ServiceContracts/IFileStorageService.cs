namespace RMS.Infrastructure.ServiceContracts;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string contentType, CancellationToken ct = default);
    Task DeleteFileAsync(string fileUrl, CancellationToken ct = default);
}