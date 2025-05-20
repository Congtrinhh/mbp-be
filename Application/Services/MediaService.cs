using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services
{
    public class MediaService : BaseService<Media, MediaDto>, IMediaService
    {
        private readonly S3Service _s3Service;
        
        public MediaService(
            IMediaRepository repository, 
            S3Service s3Service,
            ICurrentUserService currentUserService) 
            : base(repository, currentUserService)
        {
            _s3Service = s3Service;
        }

        public override async Task<int> AddAsync(MediaDto entity)
        {
            //save file to S3
            if (entity.File == null) throw new Exception("file to upload cannot be null");

            string prefix = string.Empty;
            switch (entity.Type)
            {
                case MediaType.Video:
                    prefix = "videos";
                    break;
                case MediaType.Audio:
                    prefix = "audios";
                    break;
                case MediaType.Image:
                    prefix = "images";
                    break;
                default:
                    break;
            }

            var fileName = $"{prefix}/{Guid.NewGuid()}";
            var file = entity.File;

            using (var stream = file.OpenReadStream())
            {
                // upload file to s3
                string imageUrl = await _s3Service.UploadFileAsync(stream, fileName, file.ContentType);
                entity.Url = imageUrl;

                // CreatedBy will be set automatically by base class
                return await base.AddAsync(entity);
            }
        }

        public override async Task<int> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null) throw new Exception("Cannot find media to delete");

            var url = entity.Url!;
            var filePath = _s3Service.GetS3ObjectPath(url);
            await _s3Service.DeleteFileAsync(filePath);

            return await base.DeleteAsync(id);
        }
    }
}
