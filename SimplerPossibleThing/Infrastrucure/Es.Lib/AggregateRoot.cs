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
        private readonly Dictionary<string, Action<object>> _actions = new Dictionary<string, Action<object>>();
        private readonly Dictionary<string, Type> _translations = new Dictionary<string, Type>();
        
        private readonly List<IEvent> _changes = new List<IEvent>();

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        protected AggregateRoot()
        {
            Initialize();
        }

        protected abstract void Initialize();

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
            if (_actions.ContainsKey(@object.GetType().Name))
            {
                _actions[@object.GetType().Name](@object);
            }
            if (isNew) _changes.Add(@object);
        }

        protected void RegisterApply<T>(Action<T> action)
        {
            _actions[typeof(T).Name]= (@event) =>
            {
                action((T)@event);
            };
            _translations[typeof(T).Name] = typeof(T);
        }

        protected void RegisterEvent<T>()
        {
            _translations[typeof(T).Name] = typeof(T);
        }

        public object Deserialize(string typeName,string data)
        {
            var type = _translations[typeName];
            return JsonConvert.DeserializeObject(data, type);
        }
    }
}
