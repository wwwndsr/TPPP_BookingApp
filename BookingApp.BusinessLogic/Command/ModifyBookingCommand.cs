using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Command
{
    public class ModifyBookingCommand : IBookingCommand
    {
        private readonly Booking _booking;
        private readonly string _newHotelName;
        private readonly string _newRoomName;
        private readonly decimal _newPrice;

        private string? _oldHotelName;  
        private string? _oldRoomName;  
        private decimal _oldPrice;

        public ModifyBookingCommand(Booking booking, string newHotelName, string newRoomName, decimal newPrice)
        {
            _booking = booking;
            _newHotelName = newHotelName;
            _newRoomName = newRoomName;
            _newPrice = newPrice;
        }

        public bool CanExecute()
        {
            return _booking.Status != "Отменена" && _booking.Status != "Завершена";
        }

        public void Execute()
        {
            if (!CanExecute())
                throw new InvalidOperationException("Невозможно изменить эту бронь");

            _oldHotelName = _booking.HotelName;
            _oldRoomName = _booking.RoomName;
            _oldPrice = _booking.TotalPrice;

            _booking.HotelName = _newHotelName;
            _booking.RoomName = _newRoomName;
            _booking.TotalPrice = _newPrice;
            _booking.Status = "Изменена";
        }

        public void Undo()
        {
            if (_oldHotelName != null)  // ✅ Добавлена проверка
                _booking.HotelName = _oldHotelName;
            if (_oldRoomName != null)   // ✅ Добавлена проверка
                _booking.RoomName = _oldRoomName;
            _booking.TotalPrice = _oldPrice;
            _booking.Status = "Новая";
        }

        public string GetDescription()
        {
            return $"Изменение брони #{_booking.Id} для {_booking.GuestName}";
        }
    }
}