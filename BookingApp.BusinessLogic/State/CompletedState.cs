using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public class CompletedState : IBookingState
    {
        public void Confirm(BookingContext context)
        {
            throw new InvalidOperationException("Невозможно подтвердить завершенную бронь");
        }

        public void Cancel(BookingContext context)
        {
            throw new InvalidOperationException("Невозможно отменить завершенную бронь");
        }

        public void Complete(BookingContext context)
        {
            throw new InvalidOperationException("Бронь уже завершена");
        }

        public string GetStatus() => "Завершена";

        public bool CanConfirm() => false;
        public bool CanCancel() => false;
        public bool CanComplete() => false;
    }
}