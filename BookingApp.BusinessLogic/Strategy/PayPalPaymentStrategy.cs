using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Strategy
{
    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        private readonly string _email;

        public PayPalPaymentStrategy(string email)
        {
            _email = email;
        }

        public bool Pay(decimal amount)
        {
            Console.WriteLine($"📱 PayPal оплата на сумму €{amount}");
            Console.WriteLine($"   Аккаунт: {_email}");
            Console.WriteLine($"   Статус: Ожидает подтверждения");

            return true;
        }

        public string GetPaymentMethod()
        {
            return "PayPal";
        }

        public string GetDescription()
        {
            return $"📱 PayPal (аккаунт: {_email})";
        }
    }
}
