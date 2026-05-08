using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.State
{
    public interface IBookingState
    {
        void Confirm(BookingContext context);
        void Cancel(BookingContext context);
        void Complete(BookingContext context);
        string GetStatus();
        bool CanConfirm();
        bool CanCancel();
        bool CanComplete();
    }
}
