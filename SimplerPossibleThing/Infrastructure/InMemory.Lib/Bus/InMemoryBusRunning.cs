using System;
using System.Collections.Generic;
using System.Text;

namespace InMemory.Bus
{
    public class InMemoryBusRunning:InMemoryBus
    {
        public override void Send(object message, DateTime? dealyed = null)
        {
            base.Send(message, dealyed);
            ForceRun();
        }
    }
}
