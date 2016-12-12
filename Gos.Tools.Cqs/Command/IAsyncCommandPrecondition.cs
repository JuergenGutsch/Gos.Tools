using System.Threading.Tasks;

namespace Gos.Tools.Cqs.Command
{
    public interface IAsyncCommandPrecondition<in TCommand> where TCommand : ICommand
    {
        Task CheckAsync(TCommand command);
    }
}