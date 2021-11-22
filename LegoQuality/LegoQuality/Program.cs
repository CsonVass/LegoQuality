using System;
using System.Collections.Generic;
using System.IO;

namespace LegoQuality
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> registeredUsers = User.ReadUsers();

            bool validEntry = false;
            string name = "";
            while (!validEntry)
            {
                Console.Write("Username: ");
                name = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();

                foreach (User u in registeredUsers)
                {
                    if (u.UserName.Equals(name) && u.Password.Equals(password))
                    {
                        validEntry = true;
                    }
                }

                if (!validEntry)
                {
                    Console.WriteLine("Helytelen belépési adatok, próbálja újra!");
                }
            }
            Console.Clear();
            Console.WriteLine($"Üdv {name}!");


            ///
            //Reading input
            ///

            string workingDirectory = Environment.CurrentDirectory;
            string inputFileName = "input.txt";
            string inputPath = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, inputFileName);

            string testFileName = "testlog.txt";
            string testpath = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, testFileName);

            string summaryFileName = "testSummary.txt";
            string summaryPath = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, summaryFileName);

            IInputSource inputSource = new MockInputSource(inputPath);
            var groupedLogWriter = new GroupedLogWriter(testpath);

            while (inputSource.HasNextInput())
            {
                String input = inputSource.GetNextInput();
                //Console.WriteLine("Beolvasva" + input);
                string[] values = input.Split(';');
                LogItem item = new LogItem()
                {
                    time = DateTimeOffset
               .FromUnixTimeSeconds(long.Parse(values[0]))
               .DateTime,
                    productionLineId = values[1],
                    elemId = values[2],
                    color = values[3],
                    size = int.Parse(values[4]),
                    elType = (ElType)int.Parse(values[5]),
                    errType = (ErrorType)int.Parse(values[6])

                };

                if(item.errType != ErrorType.Error0)
                {
                    Console.WriteLine($"HIBA! Gyártósor: {item.productionLineId}");
                    groupedLogWriter.AddLogItem(item);
                }
                else
                {
                    groupedLogWriter.GoodItemFound();
                }

                
            }

           
            groupedLogWriter.WriteToFile();

            SummaryCreator sc = new SummaryCreator();
            sc.CreateSummaryFromLogFile(testpath, summaryPath);

            Console.ReadKey();
        }

    }
}
