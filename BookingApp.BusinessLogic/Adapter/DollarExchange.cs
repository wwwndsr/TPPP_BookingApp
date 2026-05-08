using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public class DollarExchange
    {
        public decimal EuroToDollar(decimal euroAmount)
        {
            return euroAmount * 1.05m;
        }
    }
}
