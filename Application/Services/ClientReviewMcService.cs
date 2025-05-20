using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class ClientReviewMcService : BaseService<ClientReviewMc, ClientReviewMcDto>, IClientReviewMcService
    {
        public ClientReviewMcService(
            IClientReviewMcRepository repository,
            ICurrentUserService currentUserService) 
            : base(repository, currentUserService)
        {
        }
    }
}
