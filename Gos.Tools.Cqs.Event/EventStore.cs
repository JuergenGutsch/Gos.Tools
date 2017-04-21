using System;

namespace Gos.Tools.Cqs.Event
{
    public class EventStore<TAggregateRoot> : IEventStore<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public void AddEvent(IEvent<TAggregateRoot> @event)
        {
            throw new NotImplementedException();
        }
        public void CreateSnapshot()
        {
            throw new NotImplementedException();
        }
        public TAggregateRoot LoadAggregate()
        {
            throw new NotImplementedException();
        }
    }
}
