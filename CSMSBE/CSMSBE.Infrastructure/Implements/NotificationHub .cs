using CSMS.Data.Interfaces;
using CSMS.Entity.CSMS_Entity;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Model.Repository;
using Microsoft.AspNetCore.SignalR;
using Speckle.Core.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Infrastructure.Implements
{
    public class NotificationHub : Hub<INotificationHubClient>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPushNotificationRepository _pushNotificationRepository;

        public NotificationHub(IUnitOfWork unitOfWork, IPushNotificationRepository pushNotificationRepository)
        {
            _unitOfWork = unitOfWork;
            _pushNotificationRepository = pushNotificationRepository;
        }

        public async Task SendNotification(string userId, string title, string body)
        {
            var notification = new PushNotification()
            {
                Title = title,
                Body = body,
                AppUserId = userId,
                SentDate = DateTime.UtcNow,
                IsRead = false
            };

            await _pushNotificationRepository.AddNotificationAsync(notification);
            await _unitOfWork.CompleteAsync();

            await Clients.User(userId).ReceiveMessage(title, body);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

    }
}
