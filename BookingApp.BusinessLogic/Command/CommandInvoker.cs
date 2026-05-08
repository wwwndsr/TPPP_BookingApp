using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.BusinessLogic.Command
{
    public class CommandInvoker
    {
        private readonly Stack<IBookingCommand> _history = new();
        private readonly Stack<IBookingCommand> _redoHistory = new();

        public void ExecuteCommand(IBookingCommand command)
        {
            if (command.CanExecute())
            {
                command.Execute();
                _history.Push(command);
                _redoHistory.Clear();
            }
        }

        public void Undo()
        {
            if (_history.Count > 0)
            {
                var command = _history.Pop();
                command.Undo();
                _redoHistory.Push(command);
            }
        }

        public void Redo()
        {
            if (_redoHistory.Count > 0)
            {
                var command = _redoHistory.Pop();
                command.Execute();
                _history.Push(command);
            }
        }

        public bool CanUndo => _history.Count > 0;
        public bool CanRedo => _redoHistory.Count > 0;

        public string GetHistory()
        {
            var result = "История команд:\n";
            foreach (var cmd in _history)
            {
                result += $"  • {cmd.GetDescription()}\n";
            }
            return result;
        }
    }
}
