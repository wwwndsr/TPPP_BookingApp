using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Proxy
{
    public interface IAdminPanel
    {
        List<Booking> GetAllBookings();
        void DeleteBooking(int id);
        string GetAdminInfo();
        int GetBookingsCount();
    }
}
