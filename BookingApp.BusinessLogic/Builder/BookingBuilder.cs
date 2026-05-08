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

        public Booking Build()
        {
            var result = _booking;
            _booking = new();
            return result;
        }
    }
}
