﻿﻿﻿using Application.Dtos.Notification;
using Application.Dtos.User;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ContractService : BaseService<Contract, ContractDto>, IContractService
    {
        private readonly INotificationService _notificationService;
        private readonly IContractRepository _contractRepository;

        public ContractService(
            IContractRepository repository,
            INotificationService notificationService,
            ICurrentUserService currentUserService)
            : base(repository, currentUserService)
        {
            _notificationService = notificationService;
            _contractRepository = repository;
        }

        protected override async Task ValidateBeforeAddingAsync(ContractDto contract)
        {
            if (contract.EventStart.HasValue && contract.EventEnd.HasValue && contract.McId.HasValue)
            {
                await ValidateEventTimeAsync(contract.EventStart.Value, contract.EventEnd.Value, contract.McId.Value, contract.IsIgnoreBufferCheck);
            }
        }

        public override async Task<int> UpdateAsync(ContractDto entity)
        {
            var original = await GetByIdAsync(entity.Id);
            var originalStatus = original.Status;

            // Validate if current user is MC or Client of this contract
            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId != entity.McId && currentUserId != entity.ClientId)
            {
                throw new UnauthorizedAccessException("Only MC or Client of this contract can update it");
            }

            var result = await base.UpdateAsync(entity);

            // send notification if the contract is canceled
            if (originalStatus == ContractStatus.InEffect
                && entity.Status == ContractStatus.Canceled)
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                if (entity.McCancelDate != null)
                {
                    await _notificationService.AddAsync(new NotificationDto
                    {
                        UserId = entity.ClientId,
                        Type = NotificationType.ContractCanceled,
                        Message = $"The contract on event {entity.EventName} is canceled.",
                        AdditionalInfo = JsonConvert.SerializeObject(new ContractCanceledAdditionalInfo
                        {
                            ContractId = entity.Id
                        }, serializerSettings),
                        IsRead = false,
                        Status = NotificationStatus.NotEditable,
                        ThumbUrl = original.Mc?.AvatarUrl
                    });
                }
                else if (entity.ClientCancelDate != null)
                {
                    await _notificationService.AddAsync(new NotificationDto
                    {
                        UserId = entity.McId,
                        Type = NotificationType.ContractCanceled,
                        Message = $"The contract on event {entity.EventName} is canceled.",
                        AdditionalInfo = JsonConvert.SerializeObject(new ContractCanceledAdditionalInfo
                        {
                            ContractId = entity.Id
                        }, serializerSettings),
                        IsRead = false,
                        Status = NotificationStatus.NotEditable,
                        ThumbUrl = original.Client?.AvatarUrl
                    });
                }
            }

            return result;
        }

        public async Task ValidateEventTimeAsync(DateTime eventStart, DateTime eventEnd, int mcId, bool? isIgnoreBufferCheck = null)
        {
            // Check for direct time conflicts
            var overlappingEvent = await _contractRepository.FindOverlappingEvent(eventStart, eventEnd, mcId);
            if (overlappingEvent != null)
            {
                throw new TimeConflictException(
                    $"Bạn đã có sự kiện {overlappingEvent.EventName} từ {overlappingEvent.EventStart:dd/MM/yyyy HH:mm} đến {overlappingEvent.EventEnd:dd/MM/yyyy HH:mm}. Vui lòng kiểm tra lại",
                    overlappingEvent.Id,
                    overlappingEvent.EventName,
                    overlappingEvent.EventStart,
                    overlappingEvent.EventEnd
                );
            }

            // Check for buffer time warnings if isIgnoreBufferCheck is false or not set
            if (isIgnoreBufferCheck != true)
            {
                var bufferEvent = await _contractRepository.FindBufferConflict(eventStart, eventEnd, mcId);
                if (bufferEvent != null)
                {
                    var isAfter = bufferEvent.EventEnd <= eventStart;
                    throw new BufferTimeWarningException(
                        $"Bạn có sự kiện {bufferEvent.EventName} diễn ra ngay {(isAfter ? "trước" : "sau")} sự kiện này 60 phút, bạn có chắc vẫn muốn nhận sự kiện này không?",
                        bufferEvent.Id,
                        bufferEvent.EventName,
                        bufferEvent.EventStart,
                        bufferEvent.EventEnd
                    );
                }
            }
        }
    }
}
