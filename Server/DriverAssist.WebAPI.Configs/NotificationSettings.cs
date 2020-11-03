using System;

namespace DriverAssist.WebAPI.Configs
{
    public class NotificationSettings
    {
        public Sms Sms { get; set; }
        public WhatsApp WhatsApp { get; set; }
    }
}
