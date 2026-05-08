using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Observer
{
    public interface IObserver
    {
        void Update(string message, string bookingDetails);
    }
}
