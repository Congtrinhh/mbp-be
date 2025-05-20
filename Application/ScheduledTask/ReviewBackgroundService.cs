using Application.Dtos.Notification;
using Application.Dtos.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Amazon.S3.Util.S3EventNotification;

namespace Application.ScheduledTask
{
    public class ReviewBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReviewBackgroundService> _logger;
        private readonly IMapper _mapper;

        public ReviewBackgroundService(IServiceProvider serviceProvider, ILogger<ReviewBackgroundService> logger, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Execute the task in the background:
        /// send notification to MC and Client to remind them to review the event
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var contractRepository = scope.ServiceProvider.GetRequiredService<IContractRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    try
                    {
                        // Get contracts that meet the criteria
                        var contracts = await contractRepository.GetContractsForReviewAsync();
                        var serializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        };
                        foreach (var contract in contracts)
                        {
                            // Send notification to MC
                            var mcNotification = new NotificationDto
                            {
                                UserId = contract.McId,
                                Message = $"Vui lòng đánh giá sự kiện '{contract.EventName}' của bạn.",
                                Type = NotificationType.ReviewReminder,
                                Status = NotificationStatus.Editable,
                                AdditionalInfo = JsonConvert.SerializeObject(new ReviewReminderAdditionalInfo
                                {
                                    ContractId = contract.Id
                                }, serializerSettings),
                            };
                            await notificationService.AddAsync(mcNotification);

                            // Send notification to Client
                            var clientNotification = new NotificationDto
                            {
                                UserId = contract.ClientId,
                                Message = $"Vui lòng đánh giá sự kiện '{contract.EventName}' của bạn.",
                                Type = NotificationType.ReviewReminder,
                                Status = NotificationStatus.Editable,
                                AdditionalInfo = JsonConvert.SerializeObject(new ReviewReminderAdditionalInfo
                                {
                                    ContractId = contract.Id
                                }, serializerSettings),
                            };
                            await notificationService.AddAsync(clientNotification);

                            // Update contract status and is_remind flag
                            contract.Status = ContractStatus.Completed;
                            contract.IsRemind = true;

                            var contractDto = _mapper.Map<ContractDto>(contract);

                            await contractRepository.UpdateAsync(contractDto);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Exception in background service: {ex.Message}");
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
