using System.Threading.Tasks;

namespace Domain.Commands
{
    public interface ICommand<T>
    {
        Task<IResult<T>> ExecuteAsync(CommandState state);
        IResult<T> Execute(CommandState state);
    }
}