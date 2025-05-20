using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services
{
    /// <summary>
    /// Implementation of ICurrentUserService to access current user information from JWT claims
    /// created by: roo 20/04/2025
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("id");
            return claim != null ? int.Parse(claim.Value) : null;
        }

        public string GetEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetFullName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("fullName")?.Value;
        }

        public string GetAvatarUrl()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("avatarUrl")?.Value;
        }

        public bool IsMc()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("isMc");
            return claim != null ? bool.Parse(claim.Value) : false;
        }

        public int? GetGender()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("gender");
            return claim != null ? int.Parse(claim.Value) : null;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}