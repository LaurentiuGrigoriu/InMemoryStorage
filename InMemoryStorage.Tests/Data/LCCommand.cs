using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryStorage.Tests.Data
{
    internal class LCCommand
    {
        public int Id { get; set; }
        public string Command { get; set; }
        public CommandStatus Status { get; set; }
        public string Observation { get; set; }
        public string ScannerName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Token { get => $"LGCTSK{Id}"; }
    }
}
