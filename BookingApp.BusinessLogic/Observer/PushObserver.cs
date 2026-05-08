using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Observer
{
    public class PushObserver : IObserver
    {
        private readonly string _deviceId;

        public PushObserver(string deviceId)
        {
            _deviceId = deviceId;
        }

        public void Update(string message, string bookingDetails)
        {
            string pushContent = $@"
┌────────────────────────────────────────┐
│              🔔 PUSH УВЕДОМЛЕНИЕ       │
├────────────────────────────────────────┤
│ Устройство: {_deviceId}
│ 
│ {message}
│
│ {bookingDetails}
│
│ {DateTime.Now:HH:mm:ss}
└────────────────────────────────────────┘";

            System.Diagnostics.Debug.WriteLine(pushContent);
            LastNotification = pushContent;
        }

        public string? LastNotification { get; private set; }
    }
}
