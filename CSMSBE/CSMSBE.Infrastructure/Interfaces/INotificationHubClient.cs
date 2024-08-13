using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Infrastructure.Interfaces
{
    public interface INotificationHubClient
    {
        Task ReceiveMessage(string title, string body);
    }
}
