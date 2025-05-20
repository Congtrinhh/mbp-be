using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Domain.Entities.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Repository thông báo
    /// </summary>
    public class NotificationRepository : BaseRepository<Notification, NotificationDto>, INotificationRepository
    { 
        public NotificationRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
        }

        /// <summary>
        /// Tạo câu lệnh WHERE cho truy vấn phân trang
        /// </summary>
        /// <param name="pagedRequest">Yêu cầu phân trang</param>
        /// <returns>Câu lệnh WHERE</returns>
        protected override string GenerateWhereClause(PagedRequest pagedRequest)
        {
            var sql = base.GenerateWhereClause(pagedRequest);
            if (pagedRequest is NotificationPagedRequest userPagedRequest)
            {
                if (userPagedRequest.UserId.HasValue)
                {
                    sql += " AND user_id = @UserId";
                }
            }
            return sql;
        }

        /// <summary>
        /// Thêm các tham số lọc vào truy vấn
        /// </summary>
        /// <param name="parameters">Các tham số động</param>
        /// <param name="pagedRequest">Yêu cầu phân trang</param>
        protected override void AddFilterParameters(DynamicParameters parameters, PagedRequest pagedRequest)
        {
            if (pagedRequest is NotificationPagedRequest userPagedRequest)
            {
                if (userPagedRequest.UserId.HasValue)
                {
                    parameters.Add("UserId", userPagedRequest.UserId);
                }
            }
        }

        /// <summary>
        /// Lấy số lượng thông báo chưa đọc của người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Số lượng thông báo chưa đọc</returns>
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var sql = "SELECT COUNT(*) FROM notification WHERE user_id = @UserId AND is_read = false";
            return await DbConnection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }
    }
}
