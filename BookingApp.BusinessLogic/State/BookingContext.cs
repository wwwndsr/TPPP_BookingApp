using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public class BookingContext
    {
        private IBookingState _state;
        private readonly Booking _booking;

        public BookingContext(Booking booking)
        {
            _booking = booking;

            // Устанавливаем начальное состояние в зависимости от статуса брони
            _state = booking.Status switch
            {
                "Новая" => new NewState(),
                "Подтверждена" => new ConfirmedState(),
                "Отменена" => new CancelledState(),
                "Завершена" => new CompletedState(),
                _ => new NewState()
            };
        }

        public void SetState(IBookingState state)
        {
            _state = state;
            _booking.Status = state.GetStatus();
        }

        public void Confirm()
        {
            _state.Confirm(this);
        }

        public void Cancel()
        {
            _state.Cancel(this);
        }

        public void Complete()
        {
            _state.Complete(this);
        }

        public string GetStatus()
        {
            return _state.GetStatus();
        }

        public bool CanConfirm() => _state.CanConfirm();
        public bool CanCancel() => _state.CanCancel();
        public bool CanComplete() => _state.CanComplete();

        public Booking GetBooking() => _booking;
    }
}
