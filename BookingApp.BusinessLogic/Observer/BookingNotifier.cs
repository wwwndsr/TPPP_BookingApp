using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Observer
{
    public class BookingNotifier
    {
        private static BookingNotifier? _instance;
        private readonly List<IObserver> _observers = new();

        private BookingNotifier() { }

        public static BookingNotifier Instance
        {
            get
            {
                _instance ??= new BookingNotifier();
                return _instance;
            }
        }

        // Подписаться на уведомления
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        // Отписаться от уведомлений
        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        // Отправить уведомление всем подписчикам
        public void Notify(string message, string bookingDetails)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message, bookingDetails);
            }
        }

        // Специальные уведомления
        public void NotifyBookingCreated(string guestName, string hotelName, decimal price)
        {
            string message = $"✅ НОВАЯ БРОНЬ!";
            string details = $"Гость: {guestName}\nОтель: {hotelName}\nСумма: €{price}";
            Notify(message, details);
        }

        public void NotifyBookingConfirmed(string guestName, int bookingId)
        {
            string message = $"📌 БРОНЬ ПОДТВЕРЖДЕНА!";
            string details = $"Гость: {guestName}\nНомер брони: {bookingId}\nСтатус: Подтверждена";
            Notify(message, details);
        }

        public void NotifyBookingCancelled(string guestName, int bookingId)
        {
            string message = $"❌ БРОНЬ ОТМЕНЕНА!";
            string details = $"Гость: {guestName}\nНомер брони: {bookingId}\nСтатус: Отменена";
            Notify(message, details);
        }
    }
}
