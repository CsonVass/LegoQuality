using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using LegoQualityTDD;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class TestUser
    {
        string testpath_invalid = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "notfound.txt");
        string testpath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "testUser.txt");


        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenFileNotFound()
        {
            User.ReadFileLines(testpath_invalid);
        }

        [TestMethod]
        public void ReadLinesFormFile()
        {
            string[] expected = { "Admin:admin", "user1:password1" };
            Assert.AreEqual(expected.ToString(), User.ReadFileLines(testpath).ToString());
        }

        [TestMethod]
        public void UserCreated()
        {
            User newUser = new User("tN", "tP");
            Assert.AreEqual("tN", newUser.Name);
            Assert.AreEqual("tP", newUser.Password);
        }

        [TestMethod]
        public void UserCompare()
        {
            User u1 = new User("n", "p");
            User u2 = new User("n", "p");
            User u3 = new User("a", "b");
            User u4 = new User("n", "b");
            User u5 = new User("a", "p");

            Assert.IsTrue(u1.Equals(u2));
            Assert.IsFalse(u1.Equals(u3));
            Assert.IsFalse(u1.Equals(u4));
            Assert.IsFalse(u1.Equals(u5));
        }

        [TestMethod]
        public void CreateUsersFromFile()
        {
            List<User> expectedList = new List<User>();
            expectedList.Add(new User("Admin", "admin"));
            expectedList.Add(new User("user1", "password1"));

            User.ReadUsers(testpath);

            Assert.AreEqual(expectedList[0].Name, User.ValidUSers[0].Name);
            Assert.AreEqual(expectedList[0].Password, User.ValidUSers[0].Password);
            Assert.AreEqual(expectedList[1].Name, User.ValidUSers[1].Name);
            Assert.AreEqual(expectedList[1].Password, User.ValidUSers[1].Password);
        }

        [TestMethod]
        public void Validation()
        {
            User.ReadUsers(testpath);
            User user = new User("user1", "password1");
            User user2 = new User("user3", "password3");
            Assert.IsTrue(User.Login(testpath, user));
            Assert.IsFalse(User.Login(testpath, user2));            
        }

        [TestMethod]
        public void ConsoleReply()
        {
            User.ReadUsers(testpath);
            User user = new User("user1", "password1");
            User user2 = new User("user3", "password3");
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                User.Login(testpath, user);
                string expected = string.Format("Sikeres belépés!{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, sw.ToString());

                sw.GetStringBuilder().Clear();

                expected = string.Format("Hibás adatok!{0}", Environment.NewLine);
                User.Login(testpath, user2);
                Assert.AreEqual<string>(expected, sw.ToString());
            }
        }
    }
}
