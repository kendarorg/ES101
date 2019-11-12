using Es03.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public T GetById<T>(Guid id) where T : IAggregateRoot
        {
            var aggregateRoot = (T)Activator.CreateInstance(typeof(T));
            var events = Events.Where(e => e.Id == id).Select(ev=>ev.Data);
            aggregateRoot.LoadFromHistory(events);
            return aggregateRoot;
        }
    }
}
