using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public class LateCheckoutDecorator : BookingDecorator
    {
        public LateCheckoutDecorator(IBookingComponent booking) : base(booking) { }

        public override string GetDescription()
        {
            return _booking.GetDescription() + " + ⏰ Поздний выезд";
        }

        public override decimal GetPrice()
        {
            return _booking.GetPrice() + 100;
        }
    }
}
