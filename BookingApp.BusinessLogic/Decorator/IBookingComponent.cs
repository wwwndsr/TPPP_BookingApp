using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Decorator
{
    public interface IBookingComponent
    {
        string GetDescription();
        decimal GetPrice();
    }
}
