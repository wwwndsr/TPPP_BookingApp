using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using BookingApp.Domain;
using BookingApp.BusinessLogic.AbstractFactory;

namespace BookingApp.BusinessLogic.Builder
{
    public interface IBookingBuilder
    {
        IBookingBuilder SetGuest(string name, string email);
        IBookingBuilder SetHotel(IHotelFactory hotel);
        IBookingBuilder SetRoom(Room room);
        Booking Build();
    }
}
