using System;
using System.Collections.Generic;
using System.Text;

namespace InMemory
{
    internal enum InMemoryState
    {
        DbNotSynchronized,
        DbSavingInProgress,
        MarkedForDeletion
    }
}
