using System;

namespace Es02.Test.Infrastructure
{
    public class EventDescriptor
    {
        public Guid Id { get; set; }
        public object Data { get; set; }
        public int Version { get; set; }
    }
}
