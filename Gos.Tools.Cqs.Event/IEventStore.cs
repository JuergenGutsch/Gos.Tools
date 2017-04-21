using System;

namespace Gos.Tools.Cqs.Event
{
    public interface IEventStore<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        void AddEvent(IEvent<TAggregateRoot> @event);

        void CreateSnapshot();

        TAggregateRoot LoadAggregate();
    }
}
