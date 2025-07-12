using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Application.Dtos.IdVerification;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Net.Http.Headers;
using AutoMapper;
using Domain.Entities;
using Application.Dtos.User;

namespace Application.Services
{
    public class UserIdVerificationService : BaseService<UserIdVerification, UserIdVerificationDto>, IUserIdVerificationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserIdVerificationRepository _verificationRepository;
        private readonly IIdInfoRepository _idInfoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IS3Service _s3Service;
        protected readonly IMapper _mapper;
        private readonly string _fptAiApiKey;
        private readonly HttpClient _httpClient;

        public UserIdVerificationService(
            IConfiguration configuration,
            IUserIdVerificationRepository verificationRepository,
            IIdInfoRepository idInfoRepository,
            IUserRepository userRepository,
            IS3Service s3Service,
            IMapper mapper,
            ICurrentUserService currentUserService) : base(verificationRepository, currentUserService)
        {
            _configuration = configuration;
            _verificationRepository = verificationRepository;
            _idInfoRepository = idInfoRepository;
            _userRepository = userRepository;
            _fptAiApiKey = _configuration["FptAi:ApiKey"];
            _httpClient = new HttpClient();
            _s3Service = s3Service;
            _mapper = mapper;
        }

        public async Task<UserIdVerificationDto> GetVerificationStatusAsync(int userId)
        {
            // Use current user ID if not specified
            userId = userId == 0 ? _currentUserService.GetUserId() ?? 0 : userId;

            var verification = await _verificationRepository.GetByUserIdAsync(userId);
            if (verification == null)
            {
                return new UserIdVerificationDto 
                { 
                    UserId = userId, 
                    CurrentStep = 0, 
                    Status = 0 
                };
            }

            var dto = _mapper.Map<UserIdVerificationDto>(verification);
            if (verification.IdInfo != null)
            {
                dto.IdInfo = _mapper.Map<IdInfoDto>(verification.IdInfo);
            }

            return dto;
        }

        public async Task<UserIdVerificationDto> UploadFacePhotoAsync(int userId, FacePhotoUploadDto photo)
        {
            // Use current user ID if not specified
            userId = userId == 0 ? _currentUserService.GetUserId() ?? 0 : userId;

            try
            {
                var imageBytes = Convert.FromBase64String(photo.ImageBase64);
                using var stream = new MemoryStream(imageBytes);

                var s3Key = $"identity-verification/{userId}/face_{DateTime.UtcNow.Ticks}.jpg";
                var s3Url = await _s3Service.UploadFileAsync(stream, s3Key, "image/jpeg");

                await _verificationRepository.UpdateImagesAsync(userId, "face", s3Url);
                await _verificationRepository.UpdateCurrentStepAsync(userId, 1);

                return await GetVerificationStatusAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to upload face photo", ex);
            }
        }

        public async Task<UserIdVerificationDto> UploadIdCardPhotoAsync(int userId, IdCardPhotoUploadDto photo)
        {
            // Use current user ID if not specified
            userId = userId == 0 ? _currentUserService.GetUserId() ?? 0 : userId;

            try
            {
                var imageBytes = Convert.FromBase64String(photo.ImageBase64);
                using var stream = new MemoryStream(imageBytes);

                // Extract info and validate card detectability
                var cardInfo = await ExtractInfoFromFptAi(imageBytes, photo.Side);

                // Upload to S3
                var s3Key = $"identity-verification/{userId}/id_{photo.Side}_{DateTime.UtcNow.Ticks}.jpg";
                var s3Url = await _s3Service.UploadFileAsync(stream, s3Key, "image/jpeg");

                // Update image URL and step
                await _verificationRepository.UpdateImagesAsync(userId, photo.Side, s3Url);
                
                if (photo.Side == "front")
                {
                    // Get face photo for comparison
                    var status = await GetVerificationStatusAsync(userId);
                    if (string.IsNullOrEmpty(status.FaceImageUrl))
                    {
                        throw new Exception("Face photo not found");
                    }
                    
                    var faceBytes = await _s3Service.DownloadFileAsync(status.FaceImageUrl);
                    
                    // Validate face match
                    var isMatch = await ValidatePhotosMatchAsync(faceBytes, imageBytes);
                    if (!isMatch)
                    {
                        throw new Exception("Face photo does not match the ID card photo");
                    }
                    
                    await _verificationRepository.UpdateCurrentStepAsync(userId, 2);
                }
                else
                {
                    await _verificationRepository.UpdateCurrentStepAsync(userId, 3);
                }

                if (photo.Side == "back")
                {
                    var status = await GetVerificationStatusAsync(userId);
                    var frontBytes = await _s3Service.DownloadFileAsync(status.IdFrontImageUrl);
                    var frontInfo = await ExtractInfoFromFptAi(frontBytes, "front");

                    var idInfo = CombineIdInfo(userId, frontInfo, cardInfo);
                    await _idInfoRepository.AddOrUpdateAsync(userId, idInfo);
                }

                return await GetVerificationStatusAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload ID card {photo.Side} photo", ex);
            }
        }

        public async Task<IdInfoDto> GetIdInfoAsync(int userId)
        {
            // Use current user ID if not specified
            userId = userId == 0 ? _currentUserService.GetUserId() ?? 0 : userId;

            var info = await _idInfoRepository.GetByUserIdAsync(userId);
            return info != null ? _mapper.Map<IdInfoDto>(info) : null;
        }

        public async Task<bool> ConfirmIdInfoAsync(int userId, IdInfoDto info)
        {
            // Use current user ID if not specified
            userId = userId == 0 ? _currentUserService.GetUserId() ?? 0 : userId;

            try
            {
                var userDto = new UserDto { Id = userId, IsVerified = true };
                await _userRepository.UpdateAsync(userDto);
                await _verificationRepository.UpdateStatusAsync(userId, 1, DateTime.UtcNow);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to confirm ID information", ex);
            }
        }

        public async Task<bool> ValidatePhotosMatchAsync(byte[] faceImageBytes, byte[] idFrontImageBytes)
        {
            try
            {
                // Call FPT AI face matching API
                using var formData = new MultipartFormDataContent();
                using var faceContent = new ByteArrayContent(faceImageBytes);
                using var idContent = new ByteArrayContent(idFrontImageBytes);

                faceContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                idContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                formData.Add(faceContent, "file[]", "face.jpg");
                formData.Add(idContent, "file[]", "id.jpg");
                formData.Headers.Add("api-key", _fptAiApiKey);

                var response = await _httpClient.PostAsync(
                    "https://api.fpt.ai/dmp/checkface/v1",
                    formData);

                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(jsonString);

                if (!json.RootElement.TryGetProperty("code", out var codeProp))
                {
                    throw new Exception("Invalid response format from FPT.AI API");
                }

                var code = codeProp.GetString();
                switch (code)
                {
                    case "200":
                        var data = json.RootElement.GetProperty("data");
                        return data.GetProperty("isMatch").GetBoolean();

                    case "407":
                        throw new Exception("Face detection failed. Please ensure faces are clearly visible in both images.");

                    case "408":
                        throw new Exception("Invalid image format. Please ensure both images are in the correct format.");

                    case "409":
                        throw new Exception("Invalid number of faces detected. Each image should contain exactly one face.");

                    default:
                        var message = json.RootElement.TryGetProperty("message", out var messageProp)
                            ? messageProp.GetString()
                            : "Unknown error";
                        throw new Exception($"FPT.AI Face Matching API error: {message}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to validate photos match", ex);
            }
        }

        private async Task<JsonElement> ExtractInfoFromFptAi(byte[] imageBytes, string side)
        {
            try
            {
                using var formData = new MultipartFormDataContent();
                using var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                
                formData.Add(imageContent, "image", "image.jpg");
                formData.Headers.Add("api-key", _fptAiApiKey);

                var response = await _httpClient.PostAsync(
                    "https://api.fpt.ai/vision/idr/vnm",
                    formData);

                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(jsonString);

                if (!response.IsSuccessStatusCode)
                {
                    var message = json.RootElement.TryGetProperty("message", out var messageProp) 
                        ? messageProp.GetString() 
                        : "Unknown error";
                    throw new Exception($"FPT.AI API error: {message}");
                }

                var errorCode = json.RootElement.TryGetProperty("errorCode", out var errorCodeProp) 
                    ? errorCodeProp.GetInt32() 
                    : -1;

                if (errorCode != 0)
                {
                    var message = json.RootElement.TryGetProperty("errorMessage", out var errorMessageProp)
                        ? errorMessageProp.GetString()
                        : $"ID card {side} is not detectable";
                    throw new Exception(message);
                }

                if (!json.RootElement.TryGetProperty("data", out var dataProp) || dataProp.GetArrayLength() == 0)
                {
                    throw new Exception($"No data returned for ID card {side}");
                }

                return dataProp[0];
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
            {
                throw new Exception($"Failed to process ID card {side}", ex);
            }
        }

        private IdInfoDto CombineIdInfo(int userId, JsonElement frontInfo, JsonElement backInfo)
        {
            return new IdInfoDto
            {
                UserId = userId,
                IdNumber = frontInfo.TryGetProperty("id", out var idProp) ? idProp.GetString() : null,
                Name = frontInfo.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null,
                Dob = frontInfo.TryGetProperty("dob", out var dobProp) && DateTime.TryParseExact(dobProp.GetString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dob) ? dob : null,
                IssueDate = backInfo.TryGetProperty("issue_date", out var issueDateProp) && DateTime.TryParseExact(issueDateProp.GetString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var issueDate) ? issueDate : null,
                Doe = frontInfo.TryGetProperty("doe", out var doeProp) && DateTime.TryParseExact(doeProp.GetString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var doe) ? doe : null,
                Sex = frontInfo.TryGetProperty("sex", out var sexProp) ? sexProp.GetString() : null,
                Nationality = frontInfo.TryGetProperty("nationality", out var nationalityProp) ? nationalityProp.GetString() : null,
                Address = frontInfo.TryGetProperty("address", out var addressProp) ? addressProp.GetString() : "",
                Home = frontInfo.TryGetProperty("home", out var homeProp) ? homeProp.GetString() : "",
                Features = backInfo.TryGetProperty("features", out var featuresProp) ? featuresProp.GetString() : null,
                IssueLoc = backInfo.TryGetProperty("issue_loc", out var issueLocProp) ? issueLocProp.GetString() : null
            };
        }
    }
}