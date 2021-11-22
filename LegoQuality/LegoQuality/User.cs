using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegoQuality
{
    class User
    {

        public User(string name, string password)
        {
            this.UserName = name;
            this.Password = password;
        }

        public string UserName { get; set; }
        public string Password { get; set; }


        public static List<User> ReadUsers()
        {
            List<User> listOfUsers = new List<User>();
            string workingDirectory = Environment.CurrentDirectory;
            string fileName = "login.txt";
            string path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, fileName);
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] tokens = line.Split(":");
                listOfUsers.Add(new User(tokens[0], tokens[1]));
            }

            return listOfUsers;
        }

    }
}
