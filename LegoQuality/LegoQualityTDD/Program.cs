using System;
using System.IO;

namespace LegoQualityTDD
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SummaryCreator sc = new SummaryCreator();
            sc.CreateSummaryFromLogFile(@"testlog.txt", @"testsummary.txt");
        }
    }
}
