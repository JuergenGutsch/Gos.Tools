using System;

namespace Gos.Tools.Cqs.Event
{
    public interface IEvent<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        int EventId { get; set; }
        int AggrgateId { get; set; }
        DateTime Timestamp { get; set; }
    }
}
