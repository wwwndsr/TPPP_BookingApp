using BookingApp.BusinessLogic.AbstractFactory;
using BookingApp.BusinessLogic.Builder;
using BookingApp.BusinessLogic.Factory;
using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Builder
{
    // Директор знает, как строить разные типы броней
    public class BookingDirector
    {
        private readonly IBookingBuilder _builder;

        public BookingDirector(IBookingBuilder builder)
        {
            _builder = builder;
        }

        // Стандартная бронь (только номер)
        public Booking BuildStandardBooking(
            string guestName,
            string email,
            IHotelFactory hotel,
            Room room)
        {
            return _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .Build();
        }
    }
}