using System;

namespace LegoQuality
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
