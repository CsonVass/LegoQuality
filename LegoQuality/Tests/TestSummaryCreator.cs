using Microsoft.VisualStudio.TestTools.UnitTesting;
using LegoQualityTDD;
using System.IO;
using System;

namespace Tests
{
    [TestClass]
    public class TestSummaryCreator
    {
        string testpath1 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testlog.txt");
        string testpath2 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testlog2.txt");

        string testsummary1 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testsummary.txt");
        string testsummary2 = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testsummary2.txt");

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenFileNotFound()
        {
            SummaryCreator sc = new SummaryCreator();
            sc.GetFileLines("sure_there_is_no_such_file.txt");
        }

        [TestMethod]
        public void OpenFile()
        {
            SummaryCreator sc = new SummaryCreator();
            var lines = sc.GetFileLines(testpath1);
            Assert.AreEqual(3, lines.Length);
            Assert.AreEqual("3", lines[0]);

            lines = sc.GetFileLines(testpath2);
            Assert.AreEqual(6, lines.Length);
            Assert.AreEqual("15", lines[0]);
        }

        [TestMethod]
        public void NumberOfElements()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[2];
            
            strArray[0] = "5";
            strArray[1] = "2323;435543;34543";
            Assert.AreEqual(5, sc.GetNumberOfElements(strArray));

            strArray[0] = "612";
            strArray[1] = "blabla";
            Assert.AreEqual(612, sc.GetNumberOfElements(strArray));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NumberOfElementsException()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[2];

            strArray[0] = "bla";
            strArray[1] = "2323;435543;34543";
            sc.GetNumberOfElements(strArray);
        }

        [TestMethod]
        public void NumberOfFaultyElements()
        {
            var sc = new SummaryCreator();
            Assert.AreEqual(4, sc.GetNumberOfFaultyElements(new string[5]));
            Assert.AreEqual(10, sc.GetNumberOfFaultyElements(new string[11]));
            Assert.AreEqual(0, sc.GetNumberOfFaultyElements(new string[1]));
            Assert.AreEqual(0, sc.GetNumberOfFaultyElements(null));
            Assert.AreEqual(0, sc.GetNumberOfFaultyElements(new string[0]));
        }

        [TestMethod]
        public void FaultyPercentage()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[2];

            strArray[0] = "5";
            strArray[1] = "2323;435543;34543";
            Assert.AreEqual(0.2, sc.GetFaultPercent(strArray));

            strArray[0] = "1";
            strArray[1] = "2323;435543;34543";
            Assert.AreEqual(1, sc.GetFaultPercent(strArray));

            strArray[0] = "0";
            strArray[1] = "2323;435543;34543";
            Assert.AreEqual(0, sc.GetFaultPercent(strArray));

            strArray = new string[5];
            strArray[0] = "16";
            strArray[1] = "2323;435543;34543";
            Assert.AreEqual(0.25, sc.GetFaultPercent(strArray));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FaultyPercentGreaterThanOne()
        {

            var sc = new SummaryCreator();
            string[] strArray = new string[5];

            strArray[0] = "1";
            strArray[1] = "2323;435543;34543";
            sc.GetFaultPercent(strArray);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FaultyPercentLessThanZero()
        {

            var sc = new SummaryCreator();
            string[] strArray = new string[5];

            strArray[0] = "-1";
            strArray[1] = "2323;435543;34543";
            sc.GetFaultPercent(strArray);
        }

        [TestMethod]
        public void ErrorTypes()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[5];

            strArray[0] = "10";
            strArray[1] = "1;1;1;1;1;1;0";
            strArray[2] = "1;1;1;1;1;1;0";
            strArray[3] = "1;1;1;1;1;1;0";
            strArray[4] = "1;1;1;1;1;1;0";
            var errors = sc.GetErrorTypes(strArray);
            Assert.AreEqual(1, errors.Values.Count);
            Assert.AreEqual(1, errors.Keys.Count);
            Assert.IsTrue(errors.ContainsKey(0));
            Assert.AreEqual(4, errors[0]);

            strArray[0] = "10";
            strArray[1] = "1;1;1;1;1;1;0";
            strArray[2] = "1;1;1;1;1;1;1";
            strArray[3] = "1;1;1;1;1;1;0";
            strArray[4] = "1;1;1;1;1;1;2";
            errors = sc.GetErrorTypes(strArray);
            Assert.AreEqual(3, errors.Values.Count);
            Assert.AreEqual(3, errors.Keys.Count);
            Assert.IsTrue(errors.ContainsKey(0));
            Assert.IsTrue(errors.ContainsKey(1));
            Assert.IsTrue(errors.ContainsKey(2));
            Assert.AreEqual(2, errors[0]);
            Assert.AreEqual(1, errors[1]);
            Assert.AreEqual(1, errors[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ErrorTypeExceptionNoSplit()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[5];

            strArray[0] = "10";
            strArray[1] = "1;1;1;1;1;1;0";
            strArray[2] = "1;1;1;1;1;1;0";
            strArray[3] = "1;1;1;11;1;0";
            strArray[4] = "1;1;1;1;1;1;0";
            sc.GetErrorTypes(strArray);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ErrorTypeException()
        {
            var sc = new SummaryCreator();
            string[] strArray = new string[5];

            strArray[0] = "10";
            strArray[1] = "1;1;1;1;1;1;0";
            strArray[2] = "1;1;1;1;1;1;a";
            strArray[3] = "1;1;1;1;1;1;0";
            strArray[4] = "1;1;1;1;1;1;0";
            sc.GetErrorTypes(strArray);
        }

        [TestMethod]
        public void CreateSummary()
        {
            var sc = new SummaryCreator();

            sc.CreateSummaryFromLogFile(testpath1, testsummary1);
            Assert.IsTrue(File.Exists(testsummary1));
            var lines = File.ReadAllLines(testsummary1);
            Assert.AreEqual(7, lines.Length);
            Assert.AreEqual("Total number of elements: 3", lines[0]);
            Assert.AreEqual("Total number of faulty elements: 2", lines[1]);
            Assert.AreEqual("Fault rate: 66.66666666666666%", lines[2]);
            Assert.AreEqual("", lines[3]);
            Assert.AreEqual("Errors by code", lines[4]);
            Assert.AreEqual("Error code: 1, number: 1, percentage: 50%", lines[5]);
            Assert.AreEqual("Error code: 2, number: 1, percentage: 50%", lines[6]);

            sc.CreateSummaryFromLogFile(testpath2, testsummary2);
            Assert.IsTrue(File.Exists(testsummary2));
            lines = File.ReadAllLines(testsummary2);
            Assert.AreEqual(8, lines.Length);
            Assert.AreEqual("Total number of elements: 15", lines[0]);
            Assert.AreEqual("Total number of faulty elements: 5", lines[1]);
            Assert.AreEqual("Fault rate: 33.33333333333333%", lines[2]);
            Assert.AreEqual("", lines[3]);
            Assert.AreEqual("Errors by code", lines[4]);
            Assert.AreEqual("Error code: 1, number: 1, percentage: 20%", lines[5]);
            Assert.AreEqual("Error code: 2, number: 3, percentage: 60%", lines[6]);
            Assert.AreEqual("Error code: 3, number: 1, percentage: 20%", lines[7]);
        }
    }
}
