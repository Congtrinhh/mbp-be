using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Dtos.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Paging;

namespace Application.Services
{
    public class BaseService<T, TDto> : IBaseService<T, TDto> where T : BaseEntity where TDto : BaseEntity
    {
        private readonly IBaseRepository<T, TDto> _repository;
        protected readonly ICurrentUserService _currentUserService;

        public BaseService(IBaseRepository<T, TDto> repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public virtual async Task<TDto> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Virtual method for validation before adding entity
        /// Override this method to add custom validation logic
        /// </summary>
        /// <param name="dto">The DTO to validate</param>
        /// <returns>Task that completes when validation is done</returns>
        protected virtual async Task ValidateBeforeAddingAsync(TDto dto)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task ValidateBeforeDeletingAsync(int id)
        {
            await Task.CompletedTask;
        }

        public virtual async Task<int> AddAsync(TDto entity)
        {
            // Set created by from current user
            entity.CreatedBy = _currentUserService.GetUserId();
            await ValidateBeforeAddingAsync(entity);
            return await _repository.AddAsync(entity);
        }

        public virtual async Task<int> UpdateAsync(TDto entity)
        {
            // Set modified by from current user
            entity.ModifiedBy = _currentUserService.GetUserId();
            return await _repository.UpdateAsync(entity);
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            await ValidateBeforeDeletingAsync(id);
            return await _repository.DeleteAsync(id);
        }

        public virtual async Task<PagedResponse<TDto>> GetPagedAsync(PagedRequest pagingRequest)
        {
            return await _repository.GetPagedAsync(pagingRequest);
        }

        public virtual async Task<TDto> FindByFieldAsync(string fieldName, object value)
        {
            return await _repository.FindByFieldAsync(fieldName, value);
        }
    }
}
