using System;
using System.Collections.Generic;
using System.Text;

namespace Es.Lib
{
    public interface ISnapshottableAggregate
    {
        void SetSnasphot(string data);
        string GetSnapshot();
        int Version { get; }
        void SaveSnapshot();
    }
}
