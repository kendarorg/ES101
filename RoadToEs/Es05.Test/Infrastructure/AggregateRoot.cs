﻿using Es05.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Es04.Test.Infrastructure
{
    public class AggregateRoot
    {
        private readonly List<IEvent> _uncommittedChanges = new List<IEvent>();
        private MethodInfo[] _allApplyMethods;

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        protected AggregateRoot()
        {
            //NSFW
            _allApplyMethods = this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(m => m.Name == "Apply" && m.GetParameters().Count() == 1)
                .ToArray();
            Version = -1;
        }

        private void InvokeApplyForEvent(IEvent @event)
        {
            //NSFW
            @event.Version = ++Version;
            var realMethod = _allApplyMethods
                    .FirstOrDefault(m => m.GetParameters()[0].ParameterType == @event.GetType());
            if (realMethod != null)
            {
                realMethod.Invoke(this, new object[] { @event });
            }
        }

        public List<IEvent> GetUncommittedChanges()
        {
            return _uncommittedChanges;
        }

        protected void ApplyChange(IEvent @event,bool isNew = true)
        {
            InvokeApplyForEvent(@event);
            if (isNew)
            {
                _uncommittedChanges.Add(@event);
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                 ApplyChange(@event, false);
            }
        }
    }
}
