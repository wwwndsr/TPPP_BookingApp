using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Prototype 
{
    public interface IBookingPrototype
    {
        Booking CloneBooking(Booking source, string newGuestName = "");
        List<Booking> CloneBookingMultiple(Booking source, int count, string namePrefix = "Копия");
    }
}
