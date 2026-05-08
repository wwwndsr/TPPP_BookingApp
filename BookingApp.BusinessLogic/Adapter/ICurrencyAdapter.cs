using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public interface ICurrencyAdapter
    {
        decimal Convert(decimal euroAmount);
        string GetCurrencySymbol();
        string GetFormattedPrice(decimal euroAmount);
    }
}
