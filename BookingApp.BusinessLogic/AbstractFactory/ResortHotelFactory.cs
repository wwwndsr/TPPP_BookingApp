using BookingApp.BusinessLogic.AbstractFactory;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.AbstractFactory
{
    public class ResortHotelFactory : IHotelFactory
    {
        public string HotelName => "Курортный отель";

        public string GetServices()
        {
            return "Завтрак, СПА, бассейн";
        }

        public decimal GetBasePrice()
        {
            return 250;
        }

        public List<string> GetAvailableServices()
        {
            return new List<string> { "Завтрак", "СПА", "бассейн" };
        }
    }
}