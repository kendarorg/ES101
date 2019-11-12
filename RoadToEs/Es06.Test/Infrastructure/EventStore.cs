using E05.Test.Infrastructure;
using Es04.Test.Infrastructure;
using Es05.Test.Infrastructure;
using Es06.Test.Infrastructure;
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

        public void Save(Guid id, IEnumerable<IEvent> events, int expectedVersion)
        {
            var lastStoredEvent = Events
                .Where(e => e.Id == id)
                .OrderByDescending(ev => ev.Version)
                .FirstOrDefault();
            var lastStoredVersion = lastStoredEvent == null ? -1 : lastStoredEvent.Version;
            var lastEventVersion = events.First().Version - 1;
            if (lastEventVersion != lastStoredVersion || expectedVersion != lastStoredVersion)
            {
                throw new ConcurrencyException();
            }
            var eventSerializer = EventsSerializer.GetEventSerializer();
            foreach (var @event in events)
            {
                var serializedEvent = eventSerializer.SerializeEvent(@event);
                Events.Add(new EventDescriptor
                {
                    Version = @event.Version,
                    Id = id,
                    Data = serializedEvent.Data,
                    Type = serializedEvent.Type
                });
                _bus.Send(@event);
            }
        }


        public T GetById<T>(Guid id) where T : AggregateRoot
        {
            var aggregateRoot = (T)Activator.CreateInstance(typeof(T));
            var events = Events.Where(e => e.Id == id).Select(ev => (IEvent)ev.Data);
            aggregateRoot.LoadFromHistory(events);
            return aggregateRoot;
        }
    }
}
