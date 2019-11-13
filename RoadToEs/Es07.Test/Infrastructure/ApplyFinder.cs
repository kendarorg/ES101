using Es04.Test.Infrastructure;
using Es05.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Es06.Test.Infrastructure
{
    public class ApplyFinder
    {
        private static ApplyFinder _applyFinder;
        private readonly static object _lock = new object();

        public static void SetApplyFinder(ApplyFinder applyFinder)
        {
            _applyFinder = applyFinder;
        }
        public static ApplyFinder GetApplyFinder()
        {
            if (_applyFinder != null)
            {
                return _applyFinder;
            }
            lock (_lock)
            {
                _applyFinder = new ApplyFinder();
                _applyFinder.Initialize();
            }
            return _applyFinder;
        }

        private readonly Dictionary<Type, Dictionary<Type, MethodInfo>> _applyList = new Dictionary<Type, Dictionary<Type, MethodInfo>>();

        private void Initialize()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic))
            {
                try
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (typeof(AggregateRoot).IsAssignableFrom(type))
                        {
                            foreach (var methodInfo in type
                                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                                .Where(m => m.Name == "Apply" && m.GetParameters().Count() == 1))
                            {
                                var methodParamType = methodInfo.GetParameters()[0].ParameterType;
                                if (typeof(IEvent).IsAssignableFrom(methodParamType))
                                {
                                    if (!_applyList.ContainsKey(type))
                                    {
                                        _applyList[type] = new Dictionary<Type, MethodInfo>();
                                    }
                                    _applyList[type][methodParamType] = methodInfo;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    //NOP
                }
            }
        }

        public virtual void ApplyEvent(AggregateRoot root,IEvent @event)
        {
            if (_applyList.ContainsKey(root.GetType()) && _applyList[root.GetType()].ContainsKey(@event.GetType()))
            {
                _applyList[root.GetType()][@event.GetType()].Invoke(root, new object[] { @event });
            }
        }
    }
}
