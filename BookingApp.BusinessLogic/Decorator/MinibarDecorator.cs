using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public class MinibarDecorator : BookingDecorator
    {
        public MinibarDecorator(IBookingComponent booking) : base(booking) { }

        public override string GetDescription()
        {
            return _booking.GetDescription() + " + 🍸 Мини-бар";
        }

        public override decimal GetPrice()
        {
            return _booking.GetPrice() + 100;
        }
    }
}
