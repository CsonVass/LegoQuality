using System;
using System.Collections.Generic;

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

            var groupedLogWriter = new GroupedLogWriter(@"testlog.txt");
            groupedLogWriter.AddLogItem(new LogItem()
            {
                time = DateTime.Now,
                productionLineId = "0",
                elemId = "0",
                color = "red",
                size = 0,
                elType = 0,
                errType = 0,
            });
            groupedLogWriter.WriteToFile();

            SummaryCreator sc = new SummaryCreator();
            sc.CreateSummaryFromLogFile(@"testlog.txt", @"testsummary.txt");

            Console.ReadKey();
        }
    }
}
