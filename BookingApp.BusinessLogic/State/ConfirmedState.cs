using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public class ConfirmedState : IBookingState
    {
        public void Confirm(BookingContext context)
        {
            throw new InvalidOperationException("Бронь уже подтверждена");
        }

        public void Cancel(BookingContext context)
        {
            Console.WriteLine("❌ Подтвержденная бронь отменена");
            context.SetState(new CancelledState());
        }

        public void Complete(BookingContext context)
        {
            Console.WriteLine("🏁 Бронь завершена");
            context.SetState(new CompletedState());
        }

        public string GetStatus() => "Подтверждена";

        public bool CanConfirm() => false;
        public bool CanCancel() => true;
        public bool CanComplete() => true;
    }
}
