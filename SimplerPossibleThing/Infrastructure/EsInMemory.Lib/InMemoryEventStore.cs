using Bus;
using Crud;
using Es.Lib;
using InMemory.Crud;
using Inventory.Shared.Events;
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
        private readonly IEsFactory _esFactory;
        private readonly IOptimisticRepository<SnapshotEntity> _snapshotsRepository;
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<int, Event>> _storage;

        public InMemoryEventStore(IBus bus,IEsFactory esFactory,IOptimisticRepository<SnapshotEntity> snapshotsRepository)
        {
            _bus = bus;
            _esFactory = esFactory;
            _snapshotsRepository = snapshotsRepository;
            _storage = new ConcurrentDictionary<Guid, ConcurrentDictionary<int, Event>>();
        }

        public T GetById<T>(T newRoot, Guid aggregateId) where T : AggregateRoot
        {
            if (!_storage.ContainsKey(aggregateId)) return default;
            var firstEventVersion = LoadSnapshotAndFirstSeekableVersion(newRoot, aggregateId);
            var allEvents = FindAllEventsStartingFromRequiredVersion(aggregateId, firstEventVersion);
            newRoot.LoadsFromHistory(allEvents);
            return newRoot;
        }


        public void Save<T>(T aggregate, int expectedVersion) where T : AggregateRoot
        {
            // try to get event descriptors list for given aggregate id
            // otherwise -> create empty dictionary
            ConcurrentDictionary<int, Event> eventDescriptors = GetOrCreateEventDescriptors(aggregate);


            // check whether latest event version matches current aggregate version
            // otherwise -> throw exception 
            VerifyVersionMatch(expectedVersion, eventDescriptors);
            var i = expectedVersion;

            // iterate through current aggregate events increasing version with each processed event
            foreach (var @event in aggregate.GetUncommittedChanges())
            {
                if (IsSnapshotRequest(@event))
                {
                    HandleSnapshotRequests(aggregate);
                }
                else
                {
                    i++;
                    SaveMessageAndSend<T>(aggregate, eventDescriptors, i, @event);
                }
            }
        }

        private void SaveMessageAndSend<T>(T aggregate, ConcurrentDictionary<int, Event> eventDescriptors, int i, IEvent @event) where T : AggregateRoot
        {
            @event.Version = i;
            var serializedEvent = _esFactory.SerializeMessage(@event);
            // push event to the event descriptors list for current aggregate
            if (!eventDescriptors.TryAdd(i, new Event
            {
                Id = aggregate.Id,
                Data = serializedEvent.Data,
                Type = serializedEvent.Type,
                Version = i
            }))
            {
                throw new ConcurrencyException();
            }

            // publish current event to the bus for further processing by subscribers
            _bus.Send(@event);
        }

        #region Standard Events

        private ConcurrentDictionary<int, Event> GetOrCreateEventDescriptors<T>(T aggregate) where T : AggregateRoot
        {
            return _storage.GetOrAdd(aggregate.Id, (key) =>
            {
                return new ConcurrentDictionary<int, Event>();
            });
        }

        private static void VerifyVersionMatch(int expectedVersion, ConcurrentDictionary<int, Event> eventDescriptors)
        {
            if (eventDescriptors.Count > 1 && VersionMatchesOrNewObject(expectedVersion, eventDescriptors))
            {
                throw new ConcurrencyException();
            }
        }

        private static bool VersionMatchesOrNewObject(int expectedVersion,
            ConcurrentDictionary<int, Event> eventDescriptors)
        {
            return eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1;
        }

        private IEnumerable<IEvent> FindAllEventsStartingFromRequiredVersion(Guid aggregateId, int firstEventVersion)
        {
            return _storage[aggregateId]
                .OrderBy(a => a.Key)
                .Where(a => a.Value.Version >= firstEventVersion)
                .Select(a => (IEvent)_esFactory.DeserializeMessage(a.Value.Type, a.Value.Data));
        }
        #endregion

        #region Snapshots
        private bool IsSnapshotRequest(IEvent @event)
        {
            return null != _snapshotsRepository && typeof(SnapshotSaved) == @event.GetType();
        }

        private void HandleSnapshotRequests<T>(T aggregate) where T : AggregateRoot
        {
            var snapshotObject = _snapshotsRepository.GetById(aggregate.Id) ?? new SnapshotEntity
            {
                Id = aggregate.Id
            };
            _snapshotsRepository.Save(new SnapshotEntity
            {
                Data = ((ISnapshottableAggregate)aggregate).GetSnapshot()
            });
        }

        private int LoadSnapshotAndFirstSeekableVersion<T>(T newRoot, Guid aggregateId) where T : AggregateRoot
        {
            var firstEventVersion = -1;
            if (null != _snapshotsRepository && typeof(ISnapshottableAggregate).IsAssignableFrom(typeof(T)))
            {
                var snapshot = _snapshotsRepository.GetById(aggregateId);
                if (snapshot != null)
                {
                    ((ISnapshottableAggregate)newRoot).SetSnasphot(snapshot.Data);
                    firstEventVersion = ((ISnapshottableAggregate)newRoot).Version;
                }
            }

            return firstEventVersion;
        }
        #endregion
    }
}
