using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.AbstractFactory
{
    // Интерфейс абстрактной фабрики
    public interface IHotelFactory
    {
        string HotelName { get; }
        string GetServices();
        decimal GetBasePrice();
        List<string> GetAvailableServices();
    }
}