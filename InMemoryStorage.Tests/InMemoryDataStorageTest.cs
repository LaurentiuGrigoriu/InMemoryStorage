using InMemory.Tests.Data;
using InMemoryStorage.Tests.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InMemory.Tests
{
    public class InMemoryDataStorageTest
    {
        private readonly InMemoryDataStorage _sut;     // storage under test

        public InMemoryDataStorageTest()
        {
            /* Note:
             * The tests run cmpletely in parallel by default
             * and they do not affect each other because
             * each test is running on its own instance of InMemoryDataStorage class 
             * so that _sut will not be reused for several tests */
            _sut = new InMemoryDataStorage();
        }

        private void AddData<T>(int numberOfItems) where T : new()
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                _sut.Add(new T());
            }
        }

        private void AddLCCommandData(int numberOfItems)
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                var entry = new LCCommand()
                {
                    Id = i,
                    Command = $"cmd {i}",
                    Status = (CommandStatus)(i % 5),
                    Observation = $"LCCommand observation {i}",
                    ScannerName = $"SCNR00{i}",
                    CreatedOn = DateTime.Now
                };

                if (_sut.Add(entry))
                {
#if DEBUG
                    Console.WriteLine($"Added LCCommand data with Id={i}");
#endif
                }
            }
        }

        [Fact]
        public void AddDifferentDataInDifferentCollections()
        {
            // data1 & data2 are similar - contain the same members
            AddData<Data1>(10);
            AddData<Data2>(100);
            Data1 data1 = new Data1() { i = 1, s = "one"};
            Data2 data2 = new Data2() { i = 2, s = "two" };
            _sut.Add(data1);
            _sut.Add(data2);

            IEnumerable<Data1> data1List = _sut.Get<Data1>();
            IEnumerable<Data2> data2List = _sut.Get<Data2>();
            // check that separate data types are recorded in separate lists
            Assert.Equal(11, data1List?.Count());
            Assert.Equal(101, data2List?.Count());

            Assert.Contains(data1, data1List);
            Assert.Contains(data2, data2List);

            Data2 data3 = new Data2() { i = 3, s = "three" };
            Assert.DoesNotContain(data3, data2List);
        }

        [Fact]
        public void GetAll()
        {
            AddLCCommandData(100);

            IEnumerable<LCCommand> commands = _sut.Get<LCCommand>();

            Assert.Equal(100, commands.Count());
            Assert.Equal(20, commands.Where(x => x.Status == CommandStatus.New).Count());
            Assert.Equal(20, commands.Where(x => x.Status == CommandStatus.Open).Count());
            Assert.Equal(20, commands.Where(x => x.Status == CommandStatus.Running).Count());
            Assert.Equal(20, commands.Where(x => x.Status == CommandStatus.Suscess).Count());
            Assert.Equal(20, commands.Where(x => x.Status == CommandStatus.Failed).Count());
        }

        [Fact]
        public void GetWithPredicate()
        {
            AddLCCommandData(100);

            IEnumerable<LCCommand> data1List = _sut.Get<LCCommand>(CommandRunning);

            Assert.Equal(20, data1List.Count());
        }

        private bool CommandRunning(object cmd)
        {
            if (cmd.GetType() != typeof(LCCommand))
            {
                throw new ArgumentException($"Wrong CommandRunning argument type {cmd.GetType()}. {typeof(LCCommand)} type was expected.");
            }

            LCCommand lcCmd = (LCCommand)cmd;

            return (lcCmd.Status == CommandStatus.Running);
        }

        [Fact]
        public void AddGetDelete()
        {
            AddLCCommandData(100);
            AddData<Data1>(200);

            Assert.Equal(100, _sut.Get<LCCommand>().Count());
            Assert.Equal(200, _sut.Get<Data1>().Count());

            _sut.Delete<Data1>(_ => true);
            Assert.Equal(0, _sut.Get<Data1>().Count());

            _sut.Delete<LCCommand>(CommandRunning);
            Assert.Equal(80, _sut.Get<LCCommand>().Count());
        }

        [Fact]
        public void AddAndGetOneMillionDataSameType()
        {
            AddLCCommandData(1_000_000);

            IEnumerable<LCCommand> commands = _sut.Get<LCCommand>();

            Assert.Equal(1000000, commands.Count());
            Assert.Equal(200000, commands.Where(x => x.Status == CommandStatus.Running).Count());
        }
    }
}