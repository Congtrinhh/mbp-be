using Application.Dtos.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationRepository : IBaseRepository<Notification, NotificationDto>
    {
        Task<int> GetUnreadCountAsync(int userId);
    }
}
