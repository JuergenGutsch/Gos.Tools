using System.Threading.Tasks;

namespace Gos.Tools.Cqs.Command
{
    public interface ICommandDispatcher
    {
        void DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand;
        Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
