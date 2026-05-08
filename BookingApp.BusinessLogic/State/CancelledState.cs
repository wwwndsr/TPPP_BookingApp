using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public class CancelledState : IBookingState
    {
        public void Confirm(BookingContext context)
        {
            throw new InvalidOperationException("Невозможно подтвердить отмененную бронь");
        }

        public void Cancel(BookingContext context)
        {
            throw new InvalidOperationException("Бронь уже отменена");
        }

        public void Complete(BookingContext context)
        {
            throw new InvalidOperationException("Невозможно завершить отмененную бронь");
        }

        public string GetStatus() => "Отменена";

        public bool CanConfirm() => false;
        public bool CanCancel() => false;
        public bool CanComplete() => false;
    }
}
