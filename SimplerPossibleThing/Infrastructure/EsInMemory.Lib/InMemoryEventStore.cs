using Bus;
using Crud;
using Es.Lib;
using InMemory.Crud;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EsInMemory.Lib
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IBus _bus;
        private readonly ConcurrentDictionary<Guid, List<Event>> _storage;

        public InMemoryEventStore(IBus bus)
        {
            _bus = bus;
            _storage = new ConcurrentDictionary<Guid, List<Event>>();
        }

        public T GetById<T>(T newRoot, Guid inventoryItemId) where T:AggregateRoot
        {
            if (!_storage.ContainsKey(inventoryItemId)) return default(T);
            var allEvents = _storage[inventoryItemId]
                .OrderBy(a => a.Version)
                .Select(a => (IEvent)newRoot.Deserialize(a.Type, a.Data));
            newRoot.LoadsFromHistory(allEvents);
            return newRoot;
        }

        public void Save<T>(T aggregate, int expectedVersion) where T : AggregateRoot
        {
            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            var eventDescriptors = _storage.GetOrAdd(aggregate.Id, (key) =>
            {
                return new List<Event>();
            });


            // check whether latest event version matches current aggregate version
            // otherwise -> throw exception
            if (eventDescriptors.Count > 1 && eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in aggregate.GetUncommittedChanges())
            {
                i++;
                @event.Version = i;

                // push event to the event descriptors list for current aggregate
                eventDescriptors.Add(new Event
                {
                    Id = aggregate.Id,
                    Data = JsonConvert.SerializeObject(@event),
                    Type = @event.GetType().Name,
                    Version = i
                });


                // publish current event to the bus for further processing by subscribers
                _bus.Send(@event);
            }
        }
    }
}
