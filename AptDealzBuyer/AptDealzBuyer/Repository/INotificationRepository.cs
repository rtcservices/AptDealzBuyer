using AptDealzBuyer.Model.Reponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    public interface INotificationRepository
    {
        Task<string> GetNotificationCount();
        Task<bool> SetUserNoficiationAsRead(string NotificationId);
    }
}
