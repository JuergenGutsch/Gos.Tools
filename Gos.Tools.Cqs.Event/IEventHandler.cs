namespace Gos.Tools.Cqs.Event
{
    public interface IEventHandler<TEvent> where TEvent : IEvent<IAggregateRoot>
    {
        void Handle(TEvent @event);
    }
}
