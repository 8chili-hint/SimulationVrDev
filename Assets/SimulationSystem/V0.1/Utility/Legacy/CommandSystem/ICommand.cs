namespace SimulationSystem.V0._1.Utility.Legacy.CommandSystem
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}