using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Mediator
{
    public class ChatMediator : IChatMediator
    {
        private readonly List<ChatUser> _users = new();

        public void AddUser(ChatUser user)
        {
            _users.Add(user);
        }

        public void RemoveUser(ChatUser user)
        {
            _users.Remove(user);
        }

        public void SendMessage(string message, ChatUser sender)
        {
            // Отправляем сообщение всем, кроме отправителя
            foreach (var user in _users.Where(u => u != sender))
            {
                user.Receive(message, sender.GetName());
            }
        }

        public List<ChatUser> GetUsers() => _users.ToList();
    }
}
