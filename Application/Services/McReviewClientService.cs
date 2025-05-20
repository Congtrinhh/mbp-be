using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class McReviewClientService : BaseService<McReviewClient, McReviewClientDto>, IMcReviewClientService
    {
        public McReviewClientService(
            IMcReviewClientRepository repository,
            ICurrentUserService currentUserService) 
            : base(repository, currentUserService)
        {
        }
    }
}
