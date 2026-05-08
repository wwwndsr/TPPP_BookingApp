using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Mediator
{
    public interface IChatMediator
    {
        void SendMessage(string message, ChatUser sender);
        void AddUser(ChatUser user);
        void RemoveUser(ChatUser user);
    }
}
