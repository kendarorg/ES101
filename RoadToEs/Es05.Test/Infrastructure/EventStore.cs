using E05.Test.Infrastructure;
using Es04.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Es02.Test.Infrastructure
{
    public class EventStore
    {
        public List<EventDescriptor> Events { get; private set; }

        private E05.Test.Infrastructure.Bus _bus;

        public EventStore(E05.Test.Infrastructure.Bus bus)
        {
            Events = new List<EventDescriptor>();
            _bus = bus;
        }

        public void Save(Guid id, IEnumerable<object> events,int expectedVersion)
        {
            var lastStoredEvent = Events
                .Where(e => e.Id == id)
                .OrderByDescending(ev => ev.Version)
                .FirstOrDefault();
            var lastVersion = lastStoredEvent == null ? -1 : lastStoredEvent.Version;
            if(lastVersion!= expectedVersion)
            {
                throw new ConcurrencyException();
            }
            foreach (var @event in events)
            {
                Events.Add(new EventDescriptor
                {
                    Version = ++lastVersion,
                    Id = id,
                    Data = @event
                });
                _bus.Send(@event);
            }
        }


        public T GetById<T>(Guid id) where T : AggregateRoot
        {
            var aggregateRoot = (T)Activator.CreateInstance(typeof(T));
            var events = Events.Where(e => e.Id == id).Select(ev=>ev.Data);
            aggregateRoot.LoadFromHistory(events);
            return aggregateRoot;
        }
    }
}
