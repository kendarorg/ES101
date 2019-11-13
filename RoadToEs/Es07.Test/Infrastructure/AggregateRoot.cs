using Es05.Test.Infrastructure;
using Es06.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Es04.Test.Infrastructure
{
    public class AggregateRoot
    {
        private int _version = -1;
        private readonly List<IEvent> _uncommittedChanges = new List<IEvent>();
        
        public Guid Id { get; protected set; }
        public int Version { get { return _version; } }

        protected AggregateRoot()
        {
            
        }

        public List<IEvent> GetUncommittedChanges()
        {
            return _uncommittedChanges;
        }

        public void ClearUncommittedChanges()
        {
            _uncommittedChanges.Clear();
        }

        protected void ApplyChange(IEvent @event,bool isNew = true)
        {
            var applyFinder = ApplyFinder.GetApplyFinder();
            applyFinder.ApplyEvent(this, @event);
            if (isNew)
            {
                _version++;
                @event.Version = _version;
                _uncommittedChanges.Add(@event);
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> events)
        {
            var applyFinder = ApplyFinder.GetApplyFinder();
            foreach (var @event in events)
            {
                _version = @event.Version;
                applyFinder.ApplyEvent(this, @event);
            }
        }
    }
}
