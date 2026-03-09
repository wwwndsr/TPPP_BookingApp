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

        // Бронь с завтраком
        public Booking BuildBookingWithBreakfast(
            string guestName,
            string email,
            IHotelFactory hotel,
            Room room)
        {
            return _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .AddBreakfast()
                .Build();
        }

        // VIP бронь (все услуги)
        public Booking BuildVipBooking(
            string guestName,
            string email,
            IHotelFactory hotel,
            Room room)
        {
            return _builder
                .SetGuest(guestName, email)
                .SetHotel(hotel)
                .SetRoom(room)
                .AddBreakfast()
                .AddTransfer()
                .AddMinibar()
                .AddLateCheckout()
                .Build();
        }

        // Кастомная бронь (выборочные услуги)
        public Booking BuildCustomBooking(
            string guestName,
            string email,
            IHotelFactory hotel,
            Room room,
            bool hasBreakfast,
            bool hasTransfer,
            bool hasMinibar,
            bool hasLateCheckout)
        {
            _builder.SetGuest(guestName, email)
                    .SetHotel(hotel)
                    .SetRoom(room);

            if (hasBreakfast) _builder.AddBreakfast();
            if (hasTransfer) _builder.AddTransfer();
            if (hasMinibar) _builder.AddMinibar();
            if (hasLateCheckout) _builder.AddLateCheckout();

            return _builder.Build();
        }
    }
}