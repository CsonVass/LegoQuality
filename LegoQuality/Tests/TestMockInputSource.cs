using Microsoft.VisualStudio.TestTools.UnitTesting;
using LegoQualityTDD;
using System.IO;
using System;

namespace Tests
{
    [TestClass]
    public class TestMockInputSource
    {

        string emptyInput = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testinput_empty.txt");
        string testInput1 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testinput1.txt");


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenFileNotFound()
        {
            MockInputSource mis = new MockInputSource("not_existing_file.txt");
        }

        [TestMethod]
        public void EmptyFileHasNextInput()
        {
            MockInputSource mis = new MockInputSource(emptyInput);
            Assert.IsFalse(mis.HasNextInput());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyFileGetNextInput()
        {
            MockInputSource mis = new MockInputSource(emptyInput);
            mis.GetNextInput();
        }

        [TestMethod]
        public void GetNextInputWithoutCheckingIfHasNextInput()
        {
            MockInputSource mis = new MockInputSource(testInput1);
            Assert.AreEqual("1637601466;2;0;red;0;0;1", mis.GetNextInput());
        }

        [TestMethod]
        public void GetNextInputWithCheckingIfHasNextInput()
        {
            MockInputSource mis = new MockInputSource(testInput1);
            Assert.IsTrue(mis.HasNextInput());
            Assert.AreEqual("1637601466;2;0;red;0;0;1", mis.GetNextInput());
        }

        [TestMethod]
        public void CheckNumberOfInputsIsCorrect()
        {
            MockInputSource mis = new MockInputSource(testInput1);
            int count = 0;
            while (mis.HasNextInput())
            {
                string foo = mis.GetNextInput();
                count++;
            }
            Assert.AreEqual(3, count);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckIfExceptionIsThrownInEndlessLoop()
        {
            MockInputSource mis = new MockInputSource(testInput1);
            while (true)
            {
                string foo = mis.GetNextInput();
            }
        }




    }
}
