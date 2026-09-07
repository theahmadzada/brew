using ChaychiMenu.Infrastructure.ServiceContracts;

using Microsoft.Extensions.Configuration;

using Minio;
using Minio.DataModel.Args;

namespace ChaychiMenu.Infrastructure.Services;

public class FileStorageService(IMinioClient minioClient, IConfiguration configuration) : IFileStorageService
{
    private readonly string _bucketName = configuration["Minio:BucketName"] ?? "menu-items";
    private readonly string _publicEndpoint = configuration["Minio:PublicEndpoint"]
                                              ?? throw new InvalidOperationException("Minio:PublicEndpoint not set");
    
    private async Task EnsureBucketExistsAsync(CancellationToken ct)
    {
        var exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName), ct);

        if (!exists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName), ct);
        }
    }
    
    public async Task<string> UploadAsync(Stream stream, string contentType, CancellationToken ct = default)
    {
        var bucketExists = await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName), ct);
        if (!bucketExists)
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName), ct);

        var extension = contentType switch
        {
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".jpg"
        };

        var objectName = $"{Guid.NewGuid()}{extension}";

        await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType), ct);

        return $"{_publicEndpoint}/{_bucketName}/{objectName}";
    }

    public async Task DeleteAsync(string fileUrl, CancellationToken ct = default)
    {
        var objectName = fileUrl.Split('/').Last();

        await minioClient.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(_bucketName).WithObject(objectName), ct);
    }
}