﻿using Application.Dtos.Notification;
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
        private readonly IClientReviewMcRepository _clientReviewMcRepository;
        private readonly IMcReviewClientRepository _mcReviewClientRepository;

        protected override async Task ValidateBeforeAddingAsync(ContractDto contract)
        {
            if (contract.EventStart.HasValue && contract.EventEnd.HasValue && contract.McId.HasValue)
            {
                await ValidateEventTimeAsync(contract.EventStart.Value, contract.EventEnd.Value, contract.McId.Value, contract.IsIgnoreBufferCheck);
            }
        }

        public ContractService(
            IContractRepository repository,
            INotificationService notificationService,
            ICurrentUserService currentUserService,
            IClientReviewMcRepository clientReviewMcRepository,
            IMcReviewClientRepository mcReviewClientRepository)
            : base(repository, currentUserService)
        {
            _notificationService = notificationService;
            _contractRepository = repository;
            _clientReviewMcRepository = clientReviewMcRepository;
            _mcReviewClientRepository = mcReviewClientRepository;
        }

        private bool IsWithin24Hours(DateTime? eventStart, DateTime? cancelTime)
        {
            if (!eventStart.HasValue || !cancelTime.HasValue)
                return false;
            return (eventStart.Value - cancelTime.Value).TotalHours <= 24;
        }

        private async Task CreateAutomaticRating(ContractDto contract, bool isMcCancelled)
        {
            if (isMcCancelled)
            {
                // MC cancelled - create MC review
                await _clientReviewMcRepository.AddAsync(new ClientReviewMcDto
                {
                    ContractId = contract.Id,
                    McId = contract.McId.Value,
                    ClientId = contract.ClientId,
                    ShortDescription = "Tự động đánh giá 1 sao do hủy sự kiện gấp",
                    DetailDescription = "MC đã hủy sự kiện trong vòng 1 ngày trước khi sự kiện diễn ra",
                    OverallPoint = 1,
                    ProPoint = 1,
                    AttitudePoint = 1,
                    ReliablePoint = 1
                });

                // Send notification to MC
                await _notificationService.AddAsync(new NotificationDto
                {
                    UserId = contract.McId,
                    Type = NotificationType.GetOneStarReviewCancelContract,
                    Message = $"Bạn đã bị đánh giá 1 sao vì hủy sự kiện {contract.EventName} trong vòng 1 ngày sự kiện diễn ra",
                    IsRead = false,
                    Status = NotificationStatus.NotEditable
                });
            }
            else
            {
                // Client cancelled - create Client review
                await _mcReviewClientRepository.AddAsync(new McReviewClientDto
                {
                    ContractId = contract.Id,
                    McId = contract.McId.Value,
                    ClientId = contract.ClientId,
                    ShortDescription = "Tự động đánh giá 1 sao do hủy sự kiện gấp",
                    DetailDescription = "Khách hàng đã hủy sự kiện trong vòng 1 ngày trước khi sự kiện diễn ra",
                    OverallPoint = 1,
                    PaymentPunctualPoint = 1,
                    ReliablePoint = 1
                });

                // Send notification to Client
                await _notificationService.AddAsync(new NotificationDto
                {
                    UserId = contract.ClientId,
                    Type = NotificationType.GetOneStarReviewCancelContract,
                    Message = $"Bạn đã bị đánh giá 1 sao vì hủy sự kiện {contract.EventName} trong vòng 1 ngày sự kiện diễn ra",
                    IsRead = false,
                    Status = NotificationStatus.NotEditable
                });
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

            // Handle contract cancellation
            if (originalStatus == ContractStatus.InEffect
                && entity.Status == ContractStatus.Canceled)
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                if (entity.McCancelDate != null)
                {
                    // MC cancelled
                    await _notificationService.AddAsync(new NotificationDto
                    {
                        UserId = entity.ClientId,
                        Type = NotificationType.ContractCanceled,
                        Message = $"Hợp đồng cho sự kiện {entity.EventName} đã bị hủy.",
                        AdditionalInfo = JsonConvert.SerializeObject(new ContractCanceledAdditionalInfo
                        {
                            ContractId = entity.Id
                        }, serializerSettings),
                        IsRead = false,
                        Status = NotificationStatus.Editable,
                        ThumbUrl = original.Mc?.AvatarUrl
                    });

                    if (IsWithin24Hours(entity.EventStart, entity.McCancelDate))
                    {
                        await CreateAutomaticRating(entity, true);
                    }
                }
                else if (entity.ClientCancelDate != null)
                {
                    // Client cancelled
                    await _notificationService.AddAsync(new NotificationDto
                    {
                        UserId = entity.McId,
                        Type = NotificationType.ContractCanceled,
                        Message = $"Hợp đồng cho sự kiện {entity.EventName} đã bị hủy.",
                        AdditionalInfo = JsonConvert.SerializeObject(new ContractCanceledAdditionalInfo
                        {
                            ContractId = entity.Id
                        }, serializerSettings),
                        IsRead = false,
                        Status = NotificationStatus.Editable,
                        ThumbUrl = original.Client?.AvatarUrl
                    });

                    if (IsWithin24Hours(entity.EventStart, entity.ClientCancelDate))
                    {
                        await CreateAutomaticRating(entity, false);
                    }
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
