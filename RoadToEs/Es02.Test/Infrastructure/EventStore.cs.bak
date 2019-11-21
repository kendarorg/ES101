using System;
using System.Collections.Generic;

namespace Es02.Test.Infrastructure
{
    public class EventStore
    {
        public List<EventDescriptor> Events { get; private set; }
        public EventStore()
        {
            Events = new List<EventDescriptor>();
        }

        public void Save(Guid id, IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                Events.Add(new EventDescriptor
                {
                    Id = id,
                    Data = @event
                });
            }
        }
    }
}
