using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Es.Lib
{
    public abstract class AggregateRoot
    {
        
        private readonly List<IEvent> _changes = new List<IEvent>();
        public IEsFactory EsTestFactory { get; set; }

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        protected AggregateRoot()
        {
            
        }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(IEvent @object)
        {
            ApplyChange(@object, true);
        }

        // push atomic aggregate changes to local history for further processing (objectStore.Saveobjects)
        private void ApplyChange(IEvent @object, bool isNew)
        {
            if (EsTestFactory != null)
            {
                EsTestFactory.Apply(this, @object);
            }
            else
            {
                EsFactory.Factory.Apply(this, @object);
            }
            if (isNew) _changes.Add(@object);
        }
    }
}
