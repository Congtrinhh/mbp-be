using Application.Dtos.IdVerification;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class RequiresSameUserFilter : IAsyncActionFilter
    {
        private readonly ICurrentUserService _currentUserService;

        public RequiresSameUserFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Try to get userId from route or query parameters
            int? userId = null;
            if (context.ActionArguments.TryGetValue("userId", out var userIdObj))
            {
                userId = userIdObj as int?;
            }

            var currentUserId = _currentUserService.GetUserId();

            // If not found, try to get from entity parameter
            if (!userId.HasValue)
            {
                var entity = context.ActionArguments.Values
                    .FirstOrDefault(v => v is BaseEntity) as BaseEntity;
                if (entity != null)
                {
                    if (entity is UserDto userDto)
                    {
                        userId = userDto.Id;
                    }
                    else if (entity is PostDto postDto)
                    {
                        userId = postDto.UserId;
                    }
                    else if (entity is ContractDto contract)
                    {

                        userId = _currentUserService.IsMc() ? contract.McId : contract.ClientId;
                    }
                    //else if (entity is UserIdVerificationDto userIdVerificationDto)
                    //{
                    //    userId = userIdVerificationDto.UserId;
                    //}

                }
            }


            // If no currentUserId (not authenticated) or userId doesn't match current user
            if (!currentUserId.HasValue || !userId.HasValue || userId.Value != currentUserId.Value)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            await next();
        }
    }
}