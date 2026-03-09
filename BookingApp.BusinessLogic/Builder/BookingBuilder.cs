using BookingApp.BusinessLogic.Builder;
using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BookingApp.BusinessLogic.Factory;
using BookingApp.BusinessLogic.AbstractFactory;


namespace BookingApp.BusinessLogic.Builder
{
    public class BookingBuilder : IBookingBuilder
    {
        private Booking _booking = new();

        public IBookingBuilder SetGuest(string name, string email)
        {
            _booking.GuestName = name;
            _booking.Email = email;
            return this;
        }

        public IBookingBuilder SetHotel(IHotelFactory hotel)
        {
            _booking.HotelName = hotel.HotelName;
            return this;
        }

        public IBookingBuilder SetRoom(Room room)
        {
            _booking.RoomName = room.Name;
            return this;
        }

        public IBookingBuilder AddBreakfast()
        {
            _booking.Services.Add("Завтрак");
            return this;
        }

        public IBookingBuilder AddTransfer()
        {
            _booking.Services.Add("Трансфер");
            return this;
        }

        public IBookingBuilder AddMinibar()
        {
            _booking.Services.Add("Мини-бар");
            return this;
        }

        public IBookingBuilder AddLateCheckout()
        {
            _booking.Services.Add("Поздний выезд");
            return this;
        }

        public Booking Build()
        {
            var result = _booking;
            _booking = new(); // сброс для следующего использования
            return result;
        }
    }
}
