namespace Domain.Commands
{
    public struct CommandState
    {
        public string Target { get; }

        public CommandState(string target)
        {
            Target = target;
        }
    }
}