using Api.Attributes;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebAPI.Controllers;

namespace Api.Controllers
{
    public class NotificationsController : BaseController<Notification, NotificationDto, NotificationPagedRequest>
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService) 
            : base(notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("unread-count/{userId}")]
        [RequiresSameUser]
        public async Task<IActionResult> GetUnreadCount(int userId)
        {
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(count);
        }
    }
}
