using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Command
{
    public class CancelBookingCommand : IBookingCommand
    {
        private readonly Booking _booking;
        private string? _previousStatus; 

        public CancelBookingCommand(Booking booking)
        {
            _booking = booking;
        }

        public bool CanExecute()
        {
            return _booking.Status != "Отменена" && _booking.Status != "Завершена";
        }

        public void Execute()
        {
            if (!CanExecute())
                throw new InvalidOperationException("Невозможно отменить эту бронь");

            _previousStatus = _booking.Status;
            _booking.Status = "Отменена";
        }

        public void Undo()
        {
            if (_previousStatus != null)  
                _booking.Status = _previousStatus;
        }

        public string GetDescription()
        {
            return $"Отмена брони #{_booking.Id} для {_booking.GuestName}";
        }
    }
}