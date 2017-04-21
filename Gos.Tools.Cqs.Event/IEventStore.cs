namespace Gos.Tools.Cqs.Event
{
    public interface IEventStore<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        void AddEvent(IEvent<TAggregateRoot> @event);

        void CreateSnapshot();

        TAggregateRoot LoadAggregate();
    }

    public class EventStore<TAggregateRoot> : IEventStore<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public void AddEvent(IEvent<TAggregateRoot> @event)
        {
            throw new NotImplemetedException();
        }
    }
}
