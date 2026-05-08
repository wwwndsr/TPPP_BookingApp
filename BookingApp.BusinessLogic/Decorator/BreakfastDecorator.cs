using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public class BreakfastDecorator : BookingDecorator
    {
        public BreakfastDecorator(IBookingComponent booking) : base(booking) { }

        public override string GetDescription()
        {
            return _booking.GetDescription() + " + 🍳 Завтрак";
        }

        public override decimal GetPrice()
        {
            return _booking.GetPrice() + 50;
        }
    }
}
