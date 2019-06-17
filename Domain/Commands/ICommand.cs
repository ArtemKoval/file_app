namespace Domain.Commands
{
    public interface ICommand
    {
        bool Execute(CommandState state);
    }
}