using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PostService : BaseService<Post, PostDto>, IPostService
    {
        public PostService(
            IPostRepository repository,
            ICurrentUserService currentUserService) 
            : base(repository, currentUserService)
        {
        }

        protected override async Task ValidateBeforeDeletingAsync(int id)
        {
            var post = await GetByIdAsync(id);
            if (post.UserId != _currentUserService.GetUserId())
            {
                throw new NotSameUserException();
            }
        }
    }
}
