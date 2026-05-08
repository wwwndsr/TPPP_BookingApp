using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public class TransferDecorator : BookingDecorator
    {
        public TransferDecorator(IBookingComponent booking) : base(booking) { }

        public override string GetDescription()
        {
            return _booking.GetDescription() + " + 🚗 Трансфер";
        }

        public override decimal GetPrice()
        {
            return _booking.GetPrice() + 150;
        }
    }
}
