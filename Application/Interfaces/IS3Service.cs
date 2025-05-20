using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<byte[]> DownloadFileAsync(string fileUrl);
        Task DeleteFileAsync(string fileName);
        string GetS3ObjectPath(string url);
    }
}
