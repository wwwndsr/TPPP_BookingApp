using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Mediator
{
    public class BotUser : ChatUser
    {
        public BotUser(IChatMediator mediator, string name) : base(mediator, name) { }

        public override void Send(string message)
        {
            Console.WriteLine($"[{_name}] (бот) отправляет: {message}");
            _mediator.SendMessage(message, this);
        }

        public override void Receive(string message, string senderName)
        {
            Console.WriteLine($"[{_name}] получил от {senderName}: {message}");
            LastMessage = $"{senderName}: {message}";
            LastMessageTime = DateTime.Now;

            // Автоответ бота
            if (message.ToLower().Contains("привет") || message.ToLower().Contains("здравствуй"))
            {
                Send("Здравствуйте! Чем могу помочь? 🤖");
            }
            else if (message.ToLower().Contains("спасибо"))
            {
                Send("Пожалуйста! Рад был помочь! 😊");
            }
            else if (message.ToLower().Contains("цена") || message.ToLower().Contains("стоимость"))
            {
                Send("Цены на номера: Одноместный - €200, Люкс - €500, Апартаменты - €800 💰");
            }
        }

        public override string GetName() => _name;
        public override string GetRole() => "🤖 Бот поддержки";
        public override string GetColor() => "#27AE60";

    }
}
