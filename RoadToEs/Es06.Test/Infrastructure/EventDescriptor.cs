using System;

namespace Es02.Test.Infrastructure
{
    public class EventDescriptor
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
}
