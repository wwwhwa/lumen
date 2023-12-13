using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace transport.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int GroupID { get; set; }

        public User()
        {
        }

        public User(string surname, string name, string login, string password, int group)
        {
            Surname = surname;
            Name = name;
            Login = login;
            Password = password;
            GroupID = group;
        }
    }
}
