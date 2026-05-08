using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Strategy
{
    public interface IPaymentStrategy
    {
        bool Pay(decimal amount);
        string GetPaymentMethod();
        string GetDescription();
    }
}
