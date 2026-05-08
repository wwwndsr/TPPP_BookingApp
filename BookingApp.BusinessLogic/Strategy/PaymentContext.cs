using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Strategy
{
    public class PaymentContext
    {
        private IPaymentStrategy? _strategy;

        public void SetStrategy(IPaymentStrategy strategy)
        {
            _strategy = strategy;
            Console.WriteLine($"Способ оплаты изменен на: {_strategy.GetPaymentMethod()}");
        }

        public bool ProcessPayment(decimal amount)
        {
            if (_strategy == null)
            {
                throw new InvalidOperationException("Способ оплаты не выбран");
            }

            Console.WriteLine($"\n--- Обработка платежа на сумму €{amount} ---");
            bool result = _strategy.Pay(amount);
            Console.WriteLine($"--- Результат: {(result ? "УСПЕШНО" : "ОШИБКА")} ---\n");

            return result;
        }

        public string GetCurrentPaymentMethod()
        {
            return _strategy?.GetPaymentMethod() ?? "Не выбран";
        }

        public string GetPaymentDescription()
        {
            return _strategy?.GetDescription() ?? "Способ оплаты не выбран";
        }
    }
}
