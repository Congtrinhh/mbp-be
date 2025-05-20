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
    public class PostRepository : BaseRepository<Post, PostDto>, IPostRepository
    {
        public PostRepository(IDbConnection dbConnection, IMapper mapper) : base(dbConnection, mapper)
        {
            UseSnakeCaseMapping<Reaction>();
            UseSnakeCaseMapping<User>();
        }

        public override async Task<PagedResponse<PostDto>> GetPagedAsync(PagedRequest basePagedRequest)
        {
            if (basePagedRequest is PostPagedRequest pagedRequest && pagedRequest.IsUseProc == true)
            {
                //prepare parameters to call the stored procedure
                var parameters = new DynamicParameters();

                parameters.Add("$PostGroup", pagedRequest.PostGroup);
                parameters.Add("$KeyWord", pagedRequest.KeyWord);
                parameters.Add("$IsGetReaction", pagedRequest.IsGetReaction);

                parameters.Add("$OrderBy", pagedRequest.Sort);
                parameters.Add("$Limit", pagedRequest.PageSize);
                parameters.Add("$Offset", pagedRequest.PageIndex * pagedRequest.PageSize);

                var multi = await DbConnection.QueryMultipleAsync("proc_post_get_paged", parameters, commandType: CommandType.StoredProcedure);

                // get posts and reactions
                List<Post> posts = multi.Read<Post>().ToList();

                var postDtos = Mapper.Map<List<PostDto>>(posts);

                var totalCount = multi.ReadSingle<int>();

                var users = multi.Read<User>().ToList();

                // map users to posts based on user id
                foreach (var post in postDtos)
                {
                    int userId = (int)post.UserId!;
                    post.User = users.FirstOrDefault(m => m.Id == userId);
                }

                if (pagedRequest.IsGetReaction == true)
                {
                    // map reactions to posts based on user id
                    var reactions = multi.Read<Reaction>().ToList();

                    foreach (var post in postDtos)
                    {
                        int postId = post.Id;
                        post.Reactions = reactions.Where(m => m.PostId == postId).ToList();
                    }
                }

                return new PagedResponse<PostDto>()
                {
                    Items = postDtos,
                    TotalCount = totalCount,
                    PageIndex = basePagedRequest.PageIndex,
                    PageSize = basePagedRequest.PageSize
                };
            }
            else
            {
                return await base.GetPagedAsync(basePagedRequest);
            }
        }
    }
}
