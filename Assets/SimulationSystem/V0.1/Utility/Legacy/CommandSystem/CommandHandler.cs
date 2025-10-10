using System.Collections.Generic;

namespace SimulationSystem.V0._1.Utility.Legacy.CommandSystem
{
    public class CommandHandler
    {
        private Stack<ICommand> _commandList = new Stack<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commandList.Push(command);
            command.Execute();
        }

        public void UndoCommand()
        {
            _commandList.Pop().Undo();
        }
    }
}
