using System;
using System.Collections.Generic;

namespace InMemory
{
    public interface IDataCollection
    {
        bool Add(object item);

        IEnumerable<object> Get(Predicate<object> condition);

        void Update(Predicate<object> condition, Predicate<object> update);

        void Delete(Predicate<object> condition);

        void Clear();

        public void IncrementGeneration(object data, bool inDepth = false);
    }
}
