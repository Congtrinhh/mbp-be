using Application.Dtos.User;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMcReviewClientRepository : IBaseRepository<McReviewClient, McReviewClientDto>
    {
    }
}
