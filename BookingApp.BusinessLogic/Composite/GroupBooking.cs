using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Composite
{
    public class GroupBooking : IBookingGroupComponent
    {
        private readonly string _groupName;
        private readonly List<IBookingGroupComponent> _bookings = new();

        public GroupBooking(string groupName)
        {
            _groupName = groupName;
        }

        // Добавить компонент (бронь или другую группу)
        public void Add(IBookingGroupComponent component)
        {
            _bookings.Add(component);
        }

        // Удалить компонент
        public void Remove(IBookingGroupComponent component)
        {
            _bookings.Remove(component);
        }

        // Получить все компоненты
        public IReadOnlyList<IBookingGroupComponent> GetComponents()
        {
            return _bookings.AsReadOnly();
        }

        public string GetName()
        {
            return $"{_groupName} (группа из {GetPeopleCount()} чел.)";
        }

        public decimal GetTotalPrice()
        {
            return _bookings.Sum(b => b.GetTotalPrice());
        }

        public int GetPeopleCount()
        {
            return _bookings.Sum(b => b.GetPeopleCount());
        }

        public string GetDetails()
        {
            var details = $"📋 ГРУППА: {_groupName}\n";
            details += $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";

            foreach (var booking in _bookings)
            {
                details += $"  {booking.GetDetails()}\n";
            }

            details += $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";
            details += $"📊 ИТОГО: {GetPeopleCount()} человек | €{GetTotalPrice()}";

            return details;
        }
    }
}
