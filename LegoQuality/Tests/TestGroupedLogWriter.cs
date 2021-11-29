using Microsoft.VisualStudio.TestTools.UnitTesting;
using LegoQualityTDD;
using System.IO;
using System;

namespace Tests
{
    [TestClass]
    public class TestGroupedLogWriter
    {
        string testpath1 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testlog.txt");
        string testpath2 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testlog2.txt");

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenFileNotFound()
        {
            var groupedLogWriter = new GroupedLogWriter();
            groupedLogWriter.LoadFromFile("sure_there_is_no_such_file.txt");
        }

        [TestMethod]
        public void OpenFile()
        {
            var groupedLogWriter1 = new GroupedLogWriter();
            groupedLogWriter1.LoadFromFile(testpath1);

            var groupedLogWriter2 = new GroupedLogWriter();
            groupedLogWriter2.LoadFromFile(testpath2);
        }

        [TestMethod]
        public void OpenFileLines()
        {
            {
                var groupedLogWriter = new GroupedLogWriter();
                groupedLogWriter.LoadFromFile(testpath1);
                var itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(2, itemCount);
            }

            {
                var groupedLogWriter = new GroupedLogWriter();
                groupedLogWriter.LoadFromFile(testpath2);
                var itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(5, itemCount);
            }
        }

        [TestMethod]
        public void AddLogItems()
        {
            {
                var groupedLogWriter = new GroupedLogWriter();

                groupedLogWriter.AddLogItem(new LogItem());
                var itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(1, itemCount);

                groupedLogWriter.AddLogItem(new LogItem());
                groupedLogWriter.AddLogItem(new LogItem());
                itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(3, itemCount);

            }

            {
                var groupedLogWriter = new GroupedLogWriter();
                groupedLogWriter.LoadFromFile(testpath1);

                var itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(2, itemCount);

                groupedLogWriter.AddLogItem(new LogItem());
                groupedLogWriter.AddLogItem(new LogItem());
                itemCount = groupedLogWriter.LogItems.Count;
                Assert.AreEqual(4, itemCount);
            }
        }

        [TestMethod]
        public void LogItemErrorType()
        {
            {
                var groupedLogWriter = new GroupedLogWriter();
                groupedLogWriter.LoadFromFile(testpath1);

                var logItems = groupedLogWriter.LogItems;
                Assert.AreEqual(ErrorType.Error1, logItems[0].errorType);
                Assert.AreEqual(ErrorType.Error2, logItems[1].errorType);
            }

            {
                var groupedLogWriter = new GroupedLogWriter();
                groupedLogWriter.LoadFromFile(testpath2);

                var logItems = groupedLogWriter.LogItems;
                Assert.AreEqual(ErrorType.Error1, logItems[0].errorType);
                Assert.AreEqual(ErrorType.Error2, logItems[1].errorType);
                Assert.AreEqual(ErrorType.Error2, logItems[2].errorType);
                Assert.AreEqual(ErrorType.Error2, logItems[3].errorType);
                Assert.AreEqual(ErrorType.Error3, logItems[4].errorType);
            }
        }

        [TestMethod]
        public void LogItemGroupByErrorType()
        {
            var groupedLogWriter = new GroupedLogWriter();

            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error2 });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error1 });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error3 });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error2 });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error3 });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error1 });

            var logItems = groupedLogWriter.LogItems;
            Assert.AreEqual(ErrorType.Error1, logItems[0].errorType);
            Assert.AreEqual(ErrorType.Error1, logItems[1].errorType);
            Assert.AreEqual(ErrorType.Error2, logItems[2].errorType);
            Assert.AreEqual(ErrorType.Error2, logItems[3].errorType);
            Assert.AreEqual(ErrorType.Error3, logItems[4].errorType);
            Assert.AreEqual(ErrorType.Error3, logItems[5].errorType);
        }

        [TestMethod]
        public void LogItemGroupByTime()
        {
            var groupedLogWriter = new GroupedLogWriter();

            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error2, time = new DateTime(2021, 11, 29, 15, 24, 51) });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error1, time = new DateTime(2021, 11, 29, 15, 23, 45) });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error3, time = new DateTime(2021, 11, 29, 15, 21, 12) });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error2, time = new DateTime(2021, 11, 29, 15, 23, 34) });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error3, time = new DateTime(2021, 11, 29, 15, 18, 28) });
            groupedLogWriter.AddLogItem(new LogItem() { errorType = ErrorType.Error1, time = new DateTime(2021, 11, 29, 15, 45, 44) });

            var logItems = groupedLogWriter.LogItems;
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 23, 45), logItems[0].time);
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 45, 44), logItems[1].time);
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 23, 34), logItems[2].time);
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 24, 51), logItems[3].time);
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 18, 28), logItems[4].time);
            Assert.AreEqual(new DateTime(2021, 11, 29, 15, 21, 12), logItems[5].time);
        }

        [TestMethod]
        public void WriteLogItems()
        {
            var groupedLogWriter = new GroupedLogWriter();
            groupedLogWriter.LoadFromFile(testpath1);

            groupedLogWriter.AddLogItem(new LogItem()
            {
                time = new DateTime(2000, 11, 29, 15, 24, 51),
                productionLineId = "4",
                elemId = "0",
                color = "blue",
                size = 0,
                elType = 0,
                errorType = ErrorType.Error1,
            });

            using var memoryStream = new MemoryStream();
            groupedLogWriter.Write(memoryStream);
            var v = System.Text.Encoding.Default.GetString(memoryStream.ToArray());

            var outputLines = v.Split('\n');
            Assert.AreEqual("3", outputLines[0]);
            Assert.AreEqual("975507891;4;0;blue;0;0;1", outputLines[1]);
            Assert.AreEqual("1637597866;2;0;red;0;0;1", outputLines[2]);
            Assert.AreEqual("1637597899;4;0;blue;0;0;2", outputLines[3]);
        }
    }
}
