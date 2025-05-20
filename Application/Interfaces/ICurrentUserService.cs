using System;

namespace Application.Interfaces
{
    /// <summary>
    /// Service to access current authenticated user information
    /// created by: roo 20/04/2025
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Get the ID of the current user
        /// </summary>
        int? GetUserId();

        /// <summary>
        /// Get the email of the current user
        /// </summary>
        string GetEmail();

        /// <summary>
        /// Get the full name of the current user
        /// </summary>
        string GetFullName();

        /// <summary>
        /// Get the avatar URL of the current user
        /// </summary>
        string GetAvatarUrl();

        /// <summary>
        /// Check if the current user is an MC
        /// </summary>
        bool IsMc();

        /// <summary>
        /// Get the gender of the current user
        /// </summary>
        int? GetGender();

        /// <summary>
        /// Check if there is an authenticated user
        /// </summary>
        bool IsAuthenticated();
    }
}