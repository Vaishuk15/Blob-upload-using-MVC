namespace BlobUpload.Services
{
    public interface IBlobUploadService
    {
        Task<string> UploadInBlob(IFormFile image);
    }
}
