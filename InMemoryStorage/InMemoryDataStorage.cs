using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InMemory
{
    /* 
     * InMerory storage, organized by data-type: each data-type has it's own data collection.
     * Any data type can be added in the storage.
     */
    public class InMemoryDataStorage
    {
        private ConcurrentDictionary<Type, IDataCollection> InMemoryData { get; }

        public InMemoryDataStorage()
        {
            InMemoryData = new ConcurrentDictionary<Type, IDataCollection>();
        }

        public bool Add<T>(T data)
        {
            if (data == null)
                return false;

            IDataCollection collection = InMemoryData.GetOrAdd(typeof(T), new DataCollection<T>());

            return (collection.Add(data));
        }

        // Return:
        //      For reference type, it returns an IEnumerable of references
        public IEnumerable<T> Get<T>(Predicate<object> condition)
        {
            if (InMemoryData.ContainsKey(typeof(T)))
            {
                return InMemoryData[typeof(T)].Get(condition).Cast<T>();
            }

            return new List<T>();
        }

        public IEnumerable<T> Get<T>()
        {
            return Get<T>(_ => true);
        }

        public void Delete<T>(Predicate<object> condition)
        {
            if (InMemoryData.ContainsKey(typeof(T)))
            {
                InMemoryData[typeof(T)].Delete(condition);
            }
        }

        public void IncrementGeneration<T>(T data)
        {
            if (InMemoryData.ContainsKey(typeof(T)))
            {
                InMemoryData[typeof(T)].IncrementGeneration(data);
            }
        }
    }
}
