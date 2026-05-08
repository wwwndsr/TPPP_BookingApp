using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Observer
{
    public class EmailObserver : IObserver
    {
        private readonly string _email;

        public EmailObserver(string email)
        {
            _email = email;
        }

        public void Update(string message, string bookingDetails)
        {
            string emailContent = $@"
╔════════════════════════════════════════╗
║            📧 EMAIL УВЕДОМЛЕНИЕ        ║
╠════════════════════════════════════════╣
║ Кому: {_email}
║ Тема: {message}
║
║ Детали:
║ {bookingDetails}
║
║ Время: {DateTime.Now:HH:mm:ss}
╚════════════════════════════════════════╝";

            System.Diagnostics.Debug.WriteLine(emailContent);

            // Сохраняем в глобальную переменную для отображения в UI
            LastNotification = emailContent;
        }

        public string? LastNotification { get; private set; }
    }
}
