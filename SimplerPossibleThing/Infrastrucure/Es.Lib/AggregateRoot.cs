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
        #region Event translation and deserialization
        private class MessageType
        {
            public Type Type { get; set; }
            public MethodInfo Method { get; set; }
        }
        private static ConcurrentDictionary<Type, Dictionary<string, MessageType>> Translators = new ConcurrentDictionary<Type, Dictionary<string, MessageType>>();
        private readonly Dictionary<string, MessageType> _translators;


        private Dictionary<string, MessageType> FindAndStoreAllApplyEventMethods(Type thisType)
        {
            var result = new Dictionary<string, MessageType>();
            foreach (var method in thisType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(a => a.Name == "Apply" && a.GetParameters().Length == 1))
            {
                var methodType = method.GetParameters()[0].ParameterType;
                result[methodType.Name] = new MessageType
                {
                    Method = method,
                    Type = methodType
                };
            }
            var hm = HandledMessages;
            for (int i = 0; i < hm.Count; i++)
            {
                if (!result.ContainsKey(hm[i].Name)) {
                    result[hm[i].Name] = new MessageType
                    {
                        Type = hm[i]
                    };
                }
            }

            return result;
        }
        
        public object Deserialize(string type, string data)
        {
            var messageType = _translators[type];
            return JsonConvert.DeserializeObject(data, messageType.Type);
        }

        private void InvokeApply(IEvent @object)
        {
            if (_translators.ContainsKey(@object.GetType().Name))
            {
                var messageType = _translators[@object.GetType().Name];
                if (messageType.Method != null)
                {
                    messageType.Method.Invoke(this, new[] { @object });
                }
            }
        }

        #endregion

        private readonly List<IEvent> _changes = new List<IEvent>();

        public abstract Guid Id { get; }
        public int Version { get; internal set; }

        protected abstract List<Type> HandledMessages { get; }

        protected AggregateRoot()
        {
            _translators = Translators.GetOrAdd(this.GetType(), (type) =>
            {
                return FindAndStoreAllApplyEventMethods(this.GetType());
            });
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
            InvokeApply(@object);
            if (isNew) _changes.Add(@object);
        }
    }
}
