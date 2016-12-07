namespace Gos.Tools.Cqs.Command
{
    public interface ICommandHandler
    { }

    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}