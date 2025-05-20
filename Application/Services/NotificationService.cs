using Application.Dtos.User;
using Application.Hubs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : BaseService<Notification, NotificationDto>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            INotificationRepository repository,
            IHubContext<NotificationHub> hubContext,
            ICurrentUserService currentUserService
        ) : base(repository, currentUserService)
        {
            _notificationRepository = repository;
            _hubContext = hubContext;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            // Optionally, you can validate if the requesting user has permission
            // to view notifications for the given userId
            if (userId != _currentUserService.GetUserId())
            {
                //throw new UnauthorizedAccessException("Cannot access notifications of other users");
                return 0;
            }

            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public override async Task<int> AddAsync(NotificationDto entity)
        {
            // CreatedBy will be set automatically by base class
            var result = await base.AddAsync(entity);

            await _hubContext.Clients.Group(entity.UserId.ToString()).SendAsync("ReceiveNotification", entity.Message);

            return result;
        }
    }
}
