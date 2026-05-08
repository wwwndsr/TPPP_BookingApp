using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Strategy
{
    public class CardPaymentStrategy : IPaymentStrategy
    {
        private readonly string _cardNumber;
        private readonly string _cardHolder;
        private readonly string _expiryDate;

        public CardPaymentStrategy(string cardNumber, string cardHolder, string expiryDate)
        {
            _cardNumber = cardNumber;
            _cardHolder = cardHolder;
            _expiryDate = expiryDate;
        }

        public bool Pay(decimal amount)
        {
            Console.WriteLine($"💳 Оплата картой {_cardNumber} на сумму €{amount}");
            Console.WriteLine($"   Владелец: {_cardHolder}");
            Console.WriteLine($"   Срок действия: {_expiryDate}");

            return true;
        }

        public string GetPaymentMethod()
        {
            return "Банковская карта";
        }

        public string GetDescription()
        {
            return $"💳 Оплата картой ****{_cardNumber[^4..]} (владелец: {_cardHolder})";
        }
    }
}
