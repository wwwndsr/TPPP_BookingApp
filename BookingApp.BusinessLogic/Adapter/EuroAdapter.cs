using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public class EuroAdapter : ICurrencyAdapter
    {
        public decimal Convert(decimal euroAmount)
        {
            return euroAmount; 
        }

        public string GetCurrencySymbol()
        {
            return "€";
        }

        public string GetFormattedPrice(decimal euroAmount)
        {
            return $"{GetCurrencySymbol()}{Convert(euroAmount)}";
        }
    }
}
