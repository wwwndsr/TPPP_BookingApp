using BookingApp.Domain.Entities;
using BookingApp.BusinessLogic.Singleton;
using System;
using System.Collections.Generic;

namespace BookingApp.BusinessLogic.Prototype
{
    public class BookingCloneService : IBookingPrototype
    {
        private readonly BookingManager _bookingManager;

        public BookingCloneService()
        {
            _bookingManager = BookingManager.Instance;
        }

        public Booking CloneBooking(Booking source, string newGuestName = "")
        {
            ArgumentNullException.ThrowIfNull(source);  

            var clone = (Booking)source.Clone();
            clone.Id = 0;
            clone.BookingDate = DateTime.Now;
            clone.Status = "Копия";

            clone.GuestName = string.IsNullOrWhiteSpace(newGuestName)
                ? $"{source.GuestName} (копия)"
                : newGuestName;

            return clone;
        }

        public List<Booking> CloneBookingMultiple(Booking source, int count, string namePrefix = "Копия")
        {
            ArgumentNullException.ThrowIfNull(source);

            if (count <= 0 || count > 50)
                throw new ArgumentException("Количество копий должно быть от 1 до 50");

            var clones = new List<Booking>(count);  

            for (int i = 1; i <= count; i++)
            {
                var clone = (Booking)source.Clone();
                clone.Id = 0;
                clone.BookingDate = DateTime.Now;
                clone.GuestName = $"{namePrefix} {i}";
                clone.Status = "Копия";

                clones.Add(clone);
                _bookingManager.AddBooking(clone);
            }

            return clones;
        }
    }
}