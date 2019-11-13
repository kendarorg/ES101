using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable RedundantLambdaSignatureParentheses
namespace InMemory.Crud
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public class Repository<T>  where T : IEntity
    {
        internal readonly IDictionary<Guid, T> Items;
        private readonly object _lock = new object();
        

        protected T Clone(T input)
        {
            if (input == null)
            {
                return default;
            }

            var tmp = JsonConvert.SerializeObject(input);
            return JsonConvert.DeserializeObject<T>(tmp);
        }

        public Repository()
        {
            Items = new Dictionary<Guid, T>();
        }

        public void Delete(Guid id)
        {
            // ReSharper disable once InvertIf
            if (Items.ContainsKey(id))
            {
                Items.Remove(id);
            }
        }

        public IEnumerable<T> GetAll(Func<T, bool> query = null)
        {
            return Items.Values
                .Where((a) => query == null || query(a))
                .Select(Clone);
        }

        public T GetById(Guid id)
        {
            return Clone(GetAll((a) => a.Id == id).FirstOrDefault());
        }

        public virtual Guid Save(T toUpdate)
        {
            lock (_lock)
            {
                if (toUpdate.Id == Guid.Empty)
                {
                    toUpdate.Id = Guid.NewGuid();
                }

                if (!Items.ContainsKey(toUpdate.Id))
                {
                    Items.Add(toUpdate.Id, Clone(toUpdate));
                }
                else
                {
                    Items[toUpdate.Id] = Clone(toUpdate);
                }
            }

            return toUpdate.Id;
        }
    }
}