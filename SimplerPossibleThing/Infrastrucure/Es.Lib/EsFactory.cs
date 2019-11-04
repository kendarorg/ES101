using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Es.Lib
{
    public class EsFactory : IEsFactory
    {
        private readonly Dictionary<string, Type> _messagesTranslations = new Dictionary<string, Type>();
        private readonly Dictionary<Type, Dictionary<Type, MethodInfo>> _applyMethods = new Dictionary<Type, Dictionary<Type, MethodInfo>>();
        private static EsFactory _factory;
        private static object _lock = new object();

        public static IEsFactory Factory
        {
            get
            {
                if (_factory != null) return _factory;
                lock (_lock)
                {
                    _factory = new EsFactory();
                    _factory.ScanAssemblies();
                }
                return _factory;
            }
        }

        public void ScanAssemblies()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
            {
                try
                {
                    foreach (var type in asm.GetTypes())
                    {
                        if (typeof(IEvent).IsAssignableFrom(type) || typeof(ICommand).IsAssignableFrom(type))
                        {
                            _messagesTranslations[type.Name] = type;
                        }
                        else if (typeof(AggregateRoot).IsAssignableFrom(type))
                        {
                            ScanAppply(type);
                        }
                    }
                }
                catch (Exception)
                {
                    //NOP
                }
            }
        }

        public object DeserializeMessage(string type, string data)
        {
            return JsonConvert.DeserializeObject(data, _messagesTranslations[type]);
        }

        public SerializedMessage SerializeMessage(object data)
        {
            return new SerializedMessage
            {
                Data = JsonConvert.SerializeObject(data),
                Type = data.GetType().Name
            };
        }

        public void Apply(AggregateRoot target, object message)
        {
            var targetType = target.GetType();
            var messageType = message.GetType();
            if (_applyMethods.ContainsKey(targetType))
            {
                if (_applyMethods[targetType].ContainsKey(messageType))
                {
                    _applyMethods[targetType][messageType].Invoke(target, new object[] { message });
                }
            }
        }

        private void ScanAppply(Type type)
        {
            _applyMethods[type] = new Dictionary<Type, MethodInfo>();
            foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (methodInfo.Name == "Apply" && IsParamMessage(methodInfo.GetParameters()))
                {
                    _applyMethods[type][methodInfo.GetParameters()[0].ParameterType] = methodInfo;
                }
            }
        }

        private bool IsParamMessage(ParameterInfo[] parameterInfo)
        {
            return parameterInfo.Length == 1 && (typeof(IEvent).IsAssignableFrom(parameterInfo[0].ParameterType) || typeof(ICommand).IsAssignableFrom(parameterInfo[0].ParameterType));
        }
    }
}
