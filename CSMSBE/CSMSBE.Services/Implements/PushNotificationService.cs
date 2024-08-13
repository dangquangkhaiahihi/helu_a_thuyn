using AutoMapper;
using CSMS.Data.Implements;
using CSMS.Data.Interfaces;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.Document;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Notification;
using CSMSBE.Core.Extensions;
using CSMSBE.Infrastructure.Implements;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Model.Repository;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Implements
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IPushNotificationRepository _pushNotificationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PushNotificationService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;
        public PushNotificationService(IPushNotificationRepository pushNotificationRepository, IMapper mapper, IUnitOfWork unitOfWork, ILogger<PushNotificationService> logger, IHubContext<NotificationHub> hubContext)
        {
            _pushNotificationRepository = pushNotificationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<Result<IEnumerable<PushNotificationDto>>> GetListNotificationsByUserId(string userId)
        {
            var notificationList = await _pushNotificationRepository.GetListNotificationsByUserIdAsync(userId, trackchanges: false);

            return Result<IEnumerable<PushNotificationDto>>.Success(_mapper.Map<IEnumerable<PushNotificationDto>>(notificationList));
        }

        public async Task<Result<PushNotificationDto>> GetNotificationById(Guid id)
        {
            var notification = await _pushNotificationRepository.GetNotificationByIdAsync(id, trackchanges: false);

            return Result<PushNotificationDto>.Success(_mapper.Map<PushNotificationDto>(notification));
        }

        public async Task<Result<string>> MarkAsRead(Guid id)
        {
            await _pushNotificationRepository.MarkAsReadAsync(id);

            var isSavedSuccess = await _unitOfWork.CompleteAsync();

            if (!isSavedSuccess)
            {
                _logger.LogError(@"Failed to mark notification with id:{id} as read", id);
                return Result<string>.Failure($"Failed to mark notification with id: {id} as read.");
            }

            return Result<string>.Success($"Marked the notification with id: {id} as read successfully."); 
        }
    }
}
