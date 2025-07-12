﻿using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : BaseService<User, UserDto>, IUserService
    {
        private readonly IS3Service _s3Service;
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository repository, 
            IS3Service s3Service,
            ICurrentUserService currentUserService) 
            : base(repository, currentUserService)
        {
            _s3Service = s3Service;
            _userRepository = repository;
        }

        /// <summary>
        /// Uploads a new avatar for the user, replacing the old one if it exists.
        /// </summary>
        /// <param name="userId">The ID of the user. If not provided, uses the current user's ID.</param>
        /// <param name="fileStream">The stream of the new avatar file.</param>
        /// <param name="fileName">The name of the new avatar file. eg: my-love.jpg</param>
        /// <param name="contentType">The content type of the new avatar file.</param>
        /// <returns>The updated user DTO with the new avatar URL.</returns>
        /// <exception cref="Exception">Thrown when the user is not found or unauthorized.</exception>
        public async Task<UserDto> UploadAvatarAsync(int userId, Stream fileStream, string fileName, string contentType)
        {
            // If userId is not provided, use current user's ID
            if (userId == 0)
            {
                userId = _currentUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated");
            }
            
            // Ensure current user can only update their own avatar
            if (userId != _currentUserService.GetUserId())
            {
                throw new UnauthorizedAccessException("Cannot update avatar for another user");
            }

            // Retrieve the user by ID
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Check if the user already has an avatar
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                var oldAvatarPath = string.Empty;
                try
                {
                    // Extract the S3 object path from the old avatar URL
                    oldAvatarPath = _s3Service.GetS3ObjectPath(user.AvatarUrl);
                }
                catch (ArgumentException e)
                {
                    // Log the exception (if logging is implemented)
                }
                // Delete the old avatar from S3 if it exists
                if (!string.IsNullOrEmpty(oldAvatarPath))
                {
                    await _s3Service.DeleteFileAsync(oldAvatarPath);
                }
            }

            // Upload the new avatar to S3 and get the new URL
            fileName = $"images/{Guid.NewGuid()}-{fileName}";
            var newAvatarUrl = await _s3Service.UploadFileAsync(fileStream, fileName, contentType);

            // Update the user's avatar URL
            user.AvatarUrl = newAvatarUrl;

            // Save the updated user to the repository
            await _userRepository.UpdateAsync(user);

            // Return the updated user DTO
            return user;
        }
    }
}
