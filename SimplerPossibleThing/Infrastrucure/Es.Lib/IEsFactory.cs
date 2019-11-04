using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public interface IEsFactory
    {
        void Apply(AggregateRoot target, object message);
        object DeserializeMessage(string type, string data);
        SerializedMessage SerializeMessage(object data);

    }
}
