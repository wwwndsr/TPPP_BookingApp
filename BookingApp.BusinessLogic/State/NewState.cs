using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public class NewState : IBookingState
    {
        public void Confirm(BookingContext context)
        {
            Console.WriteLine("✅ Бронь подтверждена");
            context.SetState(new ConfirmedState());
        }

        public void Cancel(BookingContext context)
        {
            Console.WriteLine("❌ Бронь отменена");
            context.SetState(new CancelledState());
        }

        public void Complete(BookingContext context)
        {
            throw new InvalidOperationException("Невозможно завершить новую бронь. Сначала подтвердите.");
        }

        public string GetStatus() => "Новая";

        public bool CanConfirm() => true;
        public bool CanCancel() => true;
        public bool CanComplete() => false;
    }
}
