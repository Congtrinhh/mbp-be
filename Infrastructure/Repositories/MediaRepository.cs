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
    public class MediaRepository : BaseRepository<Media, MediaDto>, IMediaRepository
    {
        public MediaRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
        }

        protected override void AddFilterParameters(DynamicParameters parameters, PagedRequest pagedRequest)
        {
            if (pagedRequest is MediaPagedRequest request)
            {
                if (request.UserId.HasValue)
                {
                    parameters.Add("UserId", request.UserId);
                }
                if (request.MediaType.HasValue)
                {
                    parameters.Add("MediaType", request.MediaType);
                }
            }
        }

        protected override string GenerateWhereClause(PagedRequest pagedRequest)
        {
            var sql = base.GenerateWhereClause(pagedRequest);
            if (pagedRequest is MediaPagedRequest request)
            {

                if (request.UserId.HasValue)
                {
                sql += " AND user_id = @UserId";
                }
                if (request.MediaType.HasValue)
                {
                    sql += " AND type IN (@MediaType)";
                }

                return sql;
            }
            return sql;
        }
    }
}
