using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Domain.Entities;

namespace BookingApp.BusinessLogic.Singleton
{
    public sealed class BookingManager
    {
        private static readonly Lazy<BookingManager> _instance =
            new(() => new BookingManager());

        private readonly List<Booking> _bookings = new();

        private BookingManager() { }

        public static BookingManager Instance => _instance.Value;

        public void AddBooking(Booking booking)
        {
            booking.Id = _bookings.Count + 1;
            booking.BookingDate = DateTime.Now;
            _bookings.Add(booking);
        }

        public IReadOnlyList<Booking> GetAllBookings() => _bookings.AsReadOnly();

        public List<Booking> CloneBooking(Booking source, int copyCount, string prefix = "Копия")
        {
            var clones = new List<Booking>();

            for (int i = 1; i <= copyCount; i++)
            {
                var clone = (Booking)source.Clone();
                clone.Id = _bookings.Count + clones.Count + 1;
                clone.GuestName = $"{prefix} {i}";
                clones.Add(clone);
                _bookings.Add(clone);
            }

            return clones;
        }

        public Booking? FindBooking(int id)
        {
            return _bookings.FirstOrDefault(b => b.Id == id);
        }
    }
}