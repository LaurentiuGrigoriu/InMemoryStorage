using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryStorage.Tests.Data
{
    public enum CommandStatus
    {
        New = 0,
        Open = 1,
        Running = 2,
        Suscess = 3,
        Failed = 4,
    };
}
