using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public abstract class BookingDecorator : IBookingComponent
    {
        protected IBookingComponent _booking;

        protected BookingDecorator(IBookingComponent booking)
        {
            _booking = booking;
        }

        public abstract string GetDescription();
        public abstract decimal GetPrice();
    }
}
