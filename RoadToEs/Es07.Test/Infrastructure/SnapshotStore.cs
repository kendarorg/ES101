using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es07.Test.Infrastructure
{
    public class SnasphotDescriptor
    {
        public Guid Id { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }
        public int Version { get; set; }
    }
    public class SnapshotStore
    {
        private Dictionary<Guid, SnasphotDescriptor> _storage = new Dictionary<Guid, SnasphotDescriptor>();

        public void SaveSnapshot(ISnapshot snapshot)
        {
            if (!_storage.ContainsKey(snapshot.Id))
            {
                _storage[snapshot.Id] = new SnasphotDescriptor
                {
                    Data = JsonConvert.SerializeObject(snapshot),
                    Id = snapshot.Id,
                    Version = snapshot.Version,
                    Type = snapshot.GetType().Name
                };
            }
            else
            {
                if (_storage[snapshot.Id].Version < snapshot.Version)
                {
                    _storage[snapshot.Id].Data = JsonConvert.SerializeObject(snapshot);
                    _storage[snapshot.Id].Version = snapshot.Version;
                }
            }
        }

        public SnasphotDescriptor GetSnapshot(Guid id)
        {
            if (_storage.ContainsKey(id))
            {
                return _storage[id];
            }
            return null;
        }
    }
}
