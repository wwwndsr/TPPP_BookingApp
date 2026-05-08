using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Composite
{
    public class IndividualBooking : IBookingGroupComponent
    {
        private readonly Booking _booking;

        public IndividualBooking(Booking booking)
        {
            _booking = booking;
        }

        public string GetName()
        {
            return $"{_booking.GuestName} ({_booking.HotelName} - {_booking.RoomName})";
        }

        public decimal GetTotalPrice()
        {
            return _booking.TotalPrice;
        }

        public int GetPeopleCount()
        {
            return 1; // Один человек
        }

        public string GetDetails()
        {
            string services = _booking.Services.Count > 0
                ? string.Join(", ", _booking.Services)
                : "нет";

            return $"👤 {_booking.GuestName} | 🏨 {_booking.HotelName} | 🛏️ {_booking.RoomName} | 🍽️ {services} | €{_booking.TotalPrice}";
        }
    }
}
