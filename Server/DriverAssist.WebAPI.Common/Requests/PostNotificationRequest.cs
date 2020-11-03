using System;

namespace DriverAssist.WebAPI.Common.Requests
{
    public class PostNotificationRequest : RequestBase
    {
        public NotificationTypeDto TypeOfNotification { get; set; }
        public string Message { get; set; }
        public Guid DriverId { get; set; }
    }
}
