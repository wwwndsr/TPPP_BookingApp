using System;
using System.Collections.Generic;
using System.Text;

using BookingApp.Domain.Entities;
using System;

namespace BookingApp.BusinessLogic.Command
{
    public class ConfirmBookingCommand : IBookingCommand
    {
        private readonly Booking _booking;
        private string? _previousStatus; 

        public ConfirmBookingCommand(Booking booking)
        {
            _booking = booking;
        }

        public bool CanExecute()
        {
            return _booking.Status == "Новая";
        }

        public void Execute()
        {
            if (!CanExecute())
                throw new InvalidOperationException("Невозможно подтвердить эту бронь");

            _previousStatus = _booking.Status;
            _booking.Status = "Подтверждена";
        }

        public void Undo()
        {
            if (_previousStatus != null)  
                _booking.Status = _previousStatus;
        }

        public string GetDescription()
        {
            return $"Подтверждение брони #{_booking.Id} для {_booking.GuestName}";
        }
    }
}