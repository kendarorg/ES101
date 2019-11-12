using Es03.Test.Infrastructure;
using Es04.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es02.Test.Infrastructure
{
    public class EventStore
    {
        public List<EventDescriptor> Events { get; private set; }
        public EventStore()
        {
            Events = new List<EventDescriptor>();
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
