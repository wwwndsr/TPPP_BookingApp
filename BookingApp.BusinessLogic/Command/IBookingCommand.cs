using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Command
{
    public interface IBookingCommand
    {
        void Execute();
        void Undo();
        string GetDescription();
        bool CanExecute();
    }
}
