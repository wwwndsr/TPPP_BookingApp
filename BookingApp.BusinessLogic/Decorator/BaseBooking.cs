using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public class BaseBooking : IBookingComponent
    {
        private readonly Booking _booking;

        public BaseBooking(Booking booking)
        {
            _booking = booking;
        }

        public string GetDescription()
        {
            return $"{_booking.HotelName} - {_booking.RoomName}";
        }

        public decimal GetPrice()
        {
            return _booking.TotalPrice;
        }
    }
}
