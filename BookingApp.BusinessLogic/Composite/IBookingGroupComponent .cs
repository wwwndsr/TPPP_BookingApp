using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Composite
{
    public interface IBookingGroupComponent
    {
        string GetName();
        decimal GetTotalPrice();
        int GetPeopleCount();
        string GetDetails();
    }
}
