using BookingApp.BusinessLogic.AbstractFactory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.AbstractFactory
{
    public class CityHotelFactory : IHotelFactory
    {
        public string HotelName => "Городской отель";

        public string GetServices()
        {
            return "Завтрак";
        }

        public decimal GetBasePrice()
        {
            return 100;
        }

        public List<string> GetAvailableServices()
        {
            return new List<string> { "Завтрак" };
        }
    }
}