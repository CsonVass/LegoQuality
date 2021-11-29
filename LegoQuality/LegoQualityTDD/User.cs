using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegoQualityTDD
{
    public class User
    {
        public User(string name, string password) {
            Name = name;
            Password = password;
        }

        public string Name { get; set; }
        public string Password { get; set; }

        public static List<User> ValidUSers { get; set; } = new List<User>();


        public static void ReadUsers(string path)
        {
            List<User> userList = new List<User>();
            string[] userLines = ReadFileLines(path);
            foreach (var line in userLines)
            {
                string[] tokens = line.Split(":");
                ValidUSers.Add(new User(tokens[0], tokens[1]));
            }
        }

        public static string[] ReadFileLines(string path)
        {
            string[] lines = File.ReadAllLines(path);
            return lines;
        }

        public static bool Login(string path, User user)
        {
            bool success = ValidUSers.Find(u => u.Equals(user)) != null;
            if (success)
            {
                Console.WriteLine("Sikeres belépés!");
            }
            else
            {
                Console.WriteLine("Hibás adatok!");
            }
            return success;
        }

        public bool Equals(User u)
        {
            return this.Name.Equals(u.Name) && this.Password.Equals(u.Password);
        }
    }
}
