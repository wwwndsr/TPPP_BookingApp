using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public class LeiExchange
    {
        public decimal EuroToLei(decimal euroAmount)
        {
            return euroAmount * 19.5m;
        }
    }
}
