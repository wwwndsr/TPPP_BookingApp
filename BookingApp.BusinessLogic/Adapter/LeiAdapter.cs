using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Adapter
{
    public class LeiAdapter : ICurrencyAdapter
    {
        private readonly LeiExchange _leiExchange;

        public LeiAdapter()
        {
            _leiExchange = new LeiExchange();
        }

        // Адаптируем вызов: Convert → EuroToLei
        public decimal Convert(decimal euroAmount)
        {
            return _leiExchange.EuroToLei(euroAmount);
        }

        public string GetCurrencySymbol()
        {
            return "Lei";
        }

        public string GetFormattedPrice(decimal euroAmount)
        {
            return $"{Convert(euroAmount)} {GetCurrencySymbol()}";
        }
    }
}
