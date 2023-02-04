using System;
using System.Collections.Generic;
using System.Text;

namespace InMemory
{
    internal class MetaData
    {
        public InMemoryState State { get; set; } = InMemoryState.DbNotSynchronized;

        public int Generation { get; set; } = 0;

        public Guid Id { get; set; } = Guid.NewGuid();

        public void IncrementGeneration()
        {
            Generation++;
        }
    }
}
