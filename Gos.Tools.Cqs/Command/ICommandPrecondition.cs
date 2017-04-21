using System.Threading.Tasks;

namespace Gos.Tools.Cqs.Command
{
    public interface ICommandPrecondition<in TCommand> where TCommand : ICommand
    {
        void Check(TCommand command);
    }
}