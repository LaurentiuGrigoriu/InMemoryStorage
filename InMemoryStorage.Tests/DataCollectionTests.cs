using System.Linq;

namespace InMemory.Tests
{
    public class DataCollectionTests
    {
        private readonly DataCollection<string> _dut = new DataCollection<string>();

        [Fact]
        public void Add_InvalidType_ThrowsArgumentEx()
        {
            // assert
            Assert.Throws<ArgumentException>(() => _dut.Add(true));
            Assert.Throws<ArgumentException>(() => _dut.Add(99));
        }

        [Fact]
        public void Add_NullArgEx()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => _dut.Add(null));
        }
    }
}
