using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Es04.Test.Infrastructure
{
    public class AggregateRoot
    {
        private readonly List<object> _uncommittedChanges = new List<object>();
        private MethodInfo[] _allApplyMethods;

        public Guid Id { get; protected set; }

        protected AggregateRoot()
        {
            //NSFW
            _allApplyMethods = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == "Apply" && m.GetParameters().Count() == 1)
                .ToArray();
        }

        private void InvokeApplyForEvent(object @event)
        {
            //NSFW
            var realMethod = _allApplyMethods
                    .FirstOrDefault(m => m.GetParameters()[0].ParameterType == @event.GetType());
            if (realMethod != null)
            {
                realMethod.Invoke(this, new object[] { @event });
            }
        }

        public List<object> GetUncommittedChanges()
        {
            return _uncommittedChanges;
        }

        protected void ApplyChange(object @event,bool isNew = true)
        {
            InvokeApplyForEvent(@event);
            if (isNew)
            {
                _uncommittedChanges.Add(@event);
            }
        }

        public void LoadFromHistory(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                InvokeApplyForEvent(@event);
            }
        }
    }
}
