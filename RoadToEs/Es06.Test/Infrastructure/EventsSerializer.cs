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
        private static EventsSerializer _serializer = new EventsSerializer();

        public static void SetEventSerializer(EventsSerializer serializer)
        {
            _serializer = serializer;
        }
        public static EventsSerializer GetEventSerializer()
        {
            return _serializer;
        }

        private Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public EventsSerializer()
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

        public IEvent DeserializeEvent(string message,string name)
        {
            var type = _types[name];
            return (IEvent)JsonConvert.DeserializeObject(message, type);
        }

        public SerializedEvent SerializeEvent(object message)
        {
            var type = message.GetType().Name;
            var data = JsonConvert.SerializeObject(message);
            return new SerializedEvent
            {
                Data = data,
                Type = type
            }
        }
    }
}
