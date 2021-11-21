using System;

namespace LegoQuality
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var groupedLogWriter = new GroupedLogWriter(@"testlog.txt");
            // groupedLogWriter.AddLogItem(new LogItem()
            // {
            //         time = DateTime.Now,
            //         productionLineId = "0",
            //         elemId = "0",
            //         color = "red",
            //         size = 0,
            //         elType = 0,
            //         errType = 0,
            // });
            groupedLogWriter.WriteToFile();

            SummaryCreator sc = new SummaryCreator();
            sc.CreateSummaryFromLogFile(@"testlog.txt", @"testsummary.txt");
        }
    }
}
