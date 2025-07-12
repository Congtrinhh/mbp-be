using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType,
            };

            var response = await _s3Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
            else
            {
                throw new Exception("Error uploading file to S3");
            }
        }

        public async Task<byte[]> DownloadFileAsync(string fileUrl)
        {
            var key = GetS3ObjectPath(fileUrl);

            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            using var response = await _s3Client.GetObjectAsync(getRequest);
            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.DeleteObjectAsync(deleteRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new Exception("Error deleting file from S3");
            }
        }

        public string GetS3ObjectPath(string url)
        {
            string pattern = $@"https://{_bucketName}\.s3\.amazonaws\.com/(.+)";
            var match = System.Text.RegularExpressions.Regex.Match(url, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new ArgumentException("The URL does not match the expected S3 bucket host.");
            }
        }
    }
}
