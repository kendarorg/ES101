using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Es04.Test.Infrastructure
{
    public class AggregateRoot
    {
        private int _version = -1;
        private List<object> _uncommittedChanges = new List<object>();
        private MethodInfo[] _allApplyMethods;

        public Guid Id { get; protected set; }
        public int Version { get { return _version; } }

        protected AggregateRoot()
        {
            _allApplyMethods = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == "Apply" && m.GetParameters().Count() == 1)
                .ToArray();
        }

        public List<object> GetUncommittedChanges()
        {
            return _uncommittedChanges;
        }

        public void ClearUncommittedChanges()
        {
            _uncommittedChanges.Clear();
        }

        protected void ApplyChange(object @event,bool isNew = true)
        {
            var realMethod = _allApplyMethods.FirstOrDefault(m => m.GetParameters()[0].ParameterType == @event.GetType());
            if (realMethod != null)
            {
                realMethod.Invoke(this, new object[] { @event });
            }
            if (isNew)
            {
                _uncommittedChanges.Add(@event);
            }
        }

        public void LoadFromHistory(IEnumerable<object> events)
        {
            
            foreach (var @event in events)
            {
                _version++;
                var realMethod = _allApplyMethods.FirstOrDefault(m => m.GetParameters()[0].ParameterType == @event.GetType());
                if (realMethod != null)
                {
                    realMethod.Invoke(this, new object[] { @event });
                }
            }
        }
    }
}
