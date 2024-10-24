using AceleraPlenoProjetoFinal.Api.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace AceleraPlenoProjetoFinal.Api.Services;

public class AzureBlobContainerService
{
    private string blobConnectionString;
    private string blobContainerName;

    public AzureBlobContainerService(IConfiguration configuration)
    {
        blobConnectionString = configuration.GetValue<string>("AzureBlobContainers:blobConnectionString");
        blobContainerName = configuration.GetValue<string>("AzureBlobContainers:blobContainerName");
    }

    public void CreateBlobContainer()
    {
        var blobServiceClient = new BlobServiceClient(blobConnectionString);

        BlobContainerClient containerClient = blobServiceClient.CreateBlobContainer(blobContainerName);
    }

    public string UploadBlobContainer(CargasUploadModel arquivoUpload, string directory)
    {
        var blobServiceClient = new BlobServiceClient(blobConnectionString);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        string newFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{arquivoUpload.Arquivo.FileName}";

        BlobClient blobClient = containerClient.GetBlobClient(directory + "/" + newFileName);

        blobClient.Upload(arquivoUpload.Arquivo.OpenReadStream(), true);

        return newFileName;
    }

    public Pageable<BlobItem> ListBlobContainer(string folderPath)
    {
        var blobServiceClient = new BlobServiceClient(blobConnectionString);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        var blobs = containerClient.GetBlobs(prefix: folderPath);

        return blobs;
    }

    public void MoveBlobContainer(string fileName, string folderPathOld, string folderPathNew)
    {
        var blobServiceClient = new BlobServiceClient(blobConnectionString);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        BlobClient sourceBlobClient = containerClient.GetBlobClient(folderPathOld + "/" + fileName);
        BlobClient destinationBlobClient = containerClient.GetBlobClient(folderPathNew + "/" + fileName);

        destinationBlobClient.StartCopyFromUri(sourceBlobClient.Uri);

        sourceBlobClient.DeleteIfExists();
    }

    public string GetSasUriBlobContainer(string blob)
    {
        var blobServiceClient = new BlobServiceClient(blobConnectionString);

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);

        BlobClient blobClient = containerClient.GetBlobClient(blob);

        BlobSasBuilder sasBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));

        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

        return sasUri.ToString();
    }
}