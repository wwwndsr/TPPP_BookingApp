using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Strategy
{
    public class CashPaymentStrategy : IPaymentStrategy
    {
        public bool Pay(decimal amount)
        {
            Console.WriteLine($"💵 Выбран способ оплаты: наличными при заселении");
            Console.WriteLine($"   Сумма к оплате: €{amount}");
            Console.WriteLine($"   Оплатите при въезде в отель");

            return true;
        }

        public string GetPaymentMethod()
        {
            return "Наличные (при заселении)";
        }

        public string GetDescription()
        {
            return "💵 Оплата наличными при заселении в отель";
        }
    }
}
