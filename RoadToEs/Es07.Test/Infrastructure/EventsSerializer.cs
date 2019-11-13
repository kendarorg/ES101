using Es05.Test.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es06.Test.Infrastructure
{
    public class SerializedEvent
    {
        public string Data { get; set; }
        public string Type { get; set; }
    }

    public class EventsSerializer
    {
        private static EventsSerializer _serializer;
        private readonly static object _lock = new object();

        public static void SetEventSerializer(EventsSerializer serializer)
        {
            _serializer = serializer;
        }
        public static EventsSerializer GetEventSerializer()
        {
            if (_serializer != null)
            {
                return _serializer;
            }
            lock (_lock)
            {
                _serializer = new EventsSerializer();
                _serializer.Initialize();
            }
            return _serializer;
        }

        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        private void Initialize()
        {
            foreach(var asm in AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic))
            {
                try
                {
                    foreach(var type in asm.GetTypes())
                    {
                        if (typeof(IEvent).IsAssignableFrom(type))
                        {
                            _types.Add(type.Name, type);
                        }
                    }
                }
                catch (Exception)
                {
                    //NOP
                }
            }
        }

        public virtual IEvent DeserializeEvent(string message,string name)
        {
            var type = _types[name];
            return (IEvent)JsonConvert.DeserializeObject(message, type);
        }

        public virtual SerializedEvent SerializeEvent(object message)
        {
            var type = message.GetType().Name;
            var data = JsonConvert.SerializeObject(message);
            return new SerializedEvent
            {
                Data = data,
                Type = type
            };
        }
    }
}
