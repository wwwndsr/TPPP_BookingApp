using BookingApp.BusinessLogic.Singleton;
using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Proxy
{
    public class RealAdminPanel : IAdminPanel
    {
        private readonly BookingManager _bookingManager;

        public RealAdminPanel()
        {
            _bookingManager = BookingManager.Instance;
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingManager.GetAllBookings().ToList();
        }

        public void DeleteBooking(int id)
        {
            //todo: логика удаления
        }

        public string GetAdminInfo()
        {
            return "Административная панель v1.0\nВсе бронирования в системе";
        }

        public int GetBookingsCount()
        {
            return _bookingManager.GetAllBookings().Count;
        }
    }
}
