using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Mediator
{
    public class ClientUser : ChatUser
    {
        public ClientUser(IChatMediator mediator, string name) : base(mediator, name) { }

        public override void Send(string message)
        {
            Console.WriteLine($"[{_name}] отправляет: {message}");
            _mediator.SendMessage(message, this);
        }

        public override void Receive(string message, string senderName)
        {
            Console.WriteLine($"[{_name}] получил от {senderName}: {message}");
            LastMessage = $"{senderName}: {message}";
            LastMessageTime = DateTime.Now;
        }

        public override string GetName() => _name;
        public override string GetRole() => "👤 Пользователь";
        public override string GetColor() => "#3498DB";

    }
}
