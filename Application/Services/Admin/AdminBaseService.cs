using AutoMapper;
using Domain.Entities;
using Domain.Entities.Paging;
using Application.Interfaces.Admin;

namespace Application.Services.Admin
{
    /// <summary>
    /// Base implementation of admin service
    /// Created by: tqcong 28/04/2025
    /// </summary>
    public class AdminBaseService<TEntity, TDto> : IAdminBaseService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : class
    {
        protected readonly IAdminBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public AdminBaseService(IAdminBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedResponse<TDto>> GetPagedAsync(BaseAdminPagedRequest request)
        {
            // Get paged data using repository
            var result = await _repository.GetPagedAsync(request);

            // Map entities to DTOs
            var dtos = _mapper.Map<List<TDto>>(result.Items);

            // Convert to paged response
            return new PagedResponse<TDto>
            {
                Items = dtos,
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var result = await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TDto>(result);
        }

        public virtual async Task<TDto> UpdateAsync(int id, TDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;

            _mapper.Map(dto, entity);
            var result = await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TDto>(result);
        }

        public virtual async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }
}