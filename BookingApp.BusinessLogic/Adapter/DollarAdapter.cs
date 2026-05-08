using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public class DollarAdapter : ICurrencyAdapter
    {
        private readonly DollarExchange _dollarExchange;

        public DollarAdapter()
        {
            _dollarExchange = new DollarExchange();
        }

        // Адаптируем вызов: Convert → EuroToDollar
        public decimal Convert(decimal euroAmount)
        {
            return _dollarExchange.EuroToDollar(euroAmount);
        }

        public string GetCurrencySymbol()
        {
            return "$";
        }

        public string GetFormattedPrice(decimal euroAmount)
        {
            return $"{GetCurrencySymbol()}{Convert(euroAmount)}";
        }
    }
}
