using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Mediator
{
    public abstract class ChatUser
    {
        protected IChatMediator _mediator;
        protected string _name;

        protected ChatUser(IChatMediator mediator, string name)
        {
            _mediator = mediator;
            _name = name;
            mediator.AddUser(this);
        }

        public abstract void Send(string message);
        public abstract void Receive(string message, string senderName);
        public abstract string GetName();
        public abstract string GetRole();
        public abstract string GetColor();
        public string? LastMessage { get; protected set; }
        public DateTime? LastMessageTime { get; protected set; }
    }
}
