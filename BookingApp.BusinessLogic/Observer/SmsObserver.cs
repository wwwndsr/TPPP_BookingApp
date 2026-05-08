using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Observer
{
    public class SmsObserver : IObserver
    {
        private readonly string _phoneNumber;

        public SmsObserver(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }

        public void Update(string message, string bookingDetails)
        {
            string smsContent = $@"
┌────────────────────────────────────────┐
│              📱 SMS УВЕДОМЛЕНИЕ        │
├────────────────────────────────────────┤
│ Номер: {_phoneNumber}
│ {message}
│
│ {bookingDetails.Replace("\n", " | ")}
│
│ {DateTime.Now:HH:mm:ss}
└────────────────────────────────────────┘";

            System.Diagnostics.Debug.WriteLine(smsContent);
            LastNotification = smsContent;
        }

        public string? LastNotification { get; private set; }
    }
}
