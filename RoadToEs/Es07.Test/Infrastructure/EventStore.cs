﻿using E05.Test.Infrastructure;
using Es04.Test.Infrastructure;
using Es05.Test.Infrastructure;
using Es06.Test.Infrastructure;
using Es07.Test.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Es02.Test.Infrastructure
{
    public class EventStore
    {
        public List<EventDescriptor> Events { get; private set; }
        private readonly E05.Test.Infrastructure.Bus _bus;
    	private EventsSerializer _eventsSerializer;
        private readonly SnapshotStore _snapshotStore;

        public EventStore(E05.Test.Infrastructure.Bus bus, EventsSerializer eventsSerializer, SnapshotStore snapshotStore)
        {
            Events = new List<EventDescriptor>();
            _bus = bus;
            _eventsSerializer = eventsSerializer;
            _snapshotStore = snapshotStore;
        }

        public void Save(AggregateRoot aggregateRoot, int expectedVersion)
        {
            var events = aggregateRoot.GetUncommittedChanges();
            var lastStoredEvent = Events
                .Where(e => e.Id == aggregateRoot.Id)
                .OrderByDescending(ev => ev.Version)
                .FirstOrDefault();
            var lastVersion = lastStoredEvent == null ? -1 : lastStoredEvent.Version;
            if (lastVersion != expectedVersion)
            {
                throw new ConcurrencyException();
            }
            foreach (var @event in events)
            {
                var serializedEvent = _eventsSerializer.SerializeEvent(@event);
                lastVersion = @event.Version;
                Events.Add(new EventDescriptor
                {
                    Version = @event.Version,
                    Id = aggregateRoot.Id,
                    Data = serializedEvent.Data,
                    Type = serializedEvent.Type
                });
                _bus.Send(@event);
            }
            var snapshottableAggregateRoot = aggregateRoot as SnapshottableAggregateRoot;
            if(snapshottableAggregateRoot!=null  && snapshottableAggregateRoot.ShouldCreateSnapshot())
            {
                var snapshot = snapshottableAggregateRoot.GetSnapshot();
                snapshot.Version = lastVersion;
                _snapshotStore.SaveSnapshot(snapshot);
            }
        }

        public T GetById<T>(Guid id) where T : AggregateRoot
        {
            var aggregateRoot = (T)Activator.CreateInstance(typeof(T));
            IEnumerable<IEvent> events;

            var snapshotData = _snapshotStore.GetSnapshot(id);
            
            if (snapshotData == null)
            {
                events = Events
                    .Where(e => e.Id == id)
                    .Select(ev => _eventsSerializer.DeserializeEvent(ev.Data, ev.Type));
            }
            else
            {
                var snapshottableAggregateRoot = aggregateRoot as SnapshottableAggregateRoot;
                snapshottableAggregateRoot.LoadSnapshot(snapshotData.Data);
                events = Events
                    .Where(e => e.Id == id && e.Version > aggregateRoot.Version)
                    .Select(ev => _eventsSerializer.DeserializeEvent(ev.Data, ev.Type));
            }
            aggregateRoot.LoadFromHistory(events);
            return aggregateRoot;
        }
    }
}
