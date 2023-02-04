using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("InMemoryStorage.Tests")]
namespace InMemory
{
    internal class DataCollection<T> : IDataCollection
    {
        private ConcurrentDictionary<T, MetaData> Collection = new ConcurrentDictionary<T, MetaData>();

        private void ValidateType(object item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            if (item.GetType() != typeof(T))
            {
                throw new ArgumentException($"Wrong argument type {item.GetType()}. {typeof(T)} type was expected.", nameof(item));
            }
        }

        // add item to collection
        public bool Add(object item)
        {
            ValidateType(item); // throw exception if invalid

            Collection.TryAdd((T)item, new MetaData());

            return true;
        }

        // get items satisfying condition
        public IEnumerable<object> Get(Predicate<object> condition)
        {
            return Collection.Keys.Cast<object>()
                .Where(x => condition(x));
        }

        // apply update for all items satisfying condition
        public void Update(Predicate<object> condition, Predicate<object> update)
        {
            Collection
            .Where(item => condition(item.Key))
            .ToList()
            .ForEach(item => update(item.Key));
        }

        // delete all items satisfying condition
        public void Delete(Predicate<object> condition)
        {
            Collection
            .Where(x => condition(x.Key))
            .ToList()
            .ForEach(item => Collection.TryRemove(item.Key, out MetaData _));
        }

        public void IncrementGeneration(object data, bool inDepth = false)
        {
            ValidateType(data); // throw exception if invalid
            
            T input = (T)data;            

            if (!inDepth)
            {
                if (Collection.ContainsKey(input))
                {
                    Collection[input].Generation++;
                }
                
                return;
            }
            else
            {
                foreach (var item in Collection)
                {
                    if (input.Equals(item.Key)) // override Equals for type
                    {
                        item.Value.IncrementGeneration();
                    }
                }
            }
        }

        // clear all data in Collection
        public void Clear()
        {
            Collection.Clear();
        }

    }
}