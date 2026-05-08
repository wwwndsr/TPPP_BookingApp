using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Proxy
{
    public class AdminProxy : IAdminPanel
    {
        private RealAdminPanel? _realPanel;
        private bool _isAuthenticated;
        private const string ADMIN_PASSWORD = "admin123"; // пароль администратора

        public bool Authenticate(string password)
        {
            _isAuthenticated = password == ADMIN_PASSWORD;

            if (_isAuthenticated)
            {
                _realPanel = new RealAdminPanel();
            }

            return _isAuthenticated;
        }

        public bool IsAuthenticated => _isAuthenticated;

        public void Logout()
        {
            _isAuthenticated = false;
            _realPanel = null;
        }

        public List<Booking> GetAllBookings()
        {
            CheckAccess();
            return _realPanel!.GetAllBookings();
        }

        public void DeleteBooking(int id)
        {
            CheckAccess();
            _realPanel!.DeleteBooking(id);
        }

        public string GetAdminInfo()
        {
            CheckAccess();
            return _realPanel!.GetAdminInfo();
        }

        public int GetBookingsCount()
        {
            CheckAccess();
            return _realPanel!.GetBookingsCount();
        }

        private void CheckAccess()
        {
            if (!_isAuthenticated)
            {
                throw new UnauthorizedAccessException("Доступ запрещен! Требуется авторизация.");
            }

            if (_realPanel == null)
            {
                throw new InvalidOperationException("Система не инициализирована");
            }
        }
    }
}
