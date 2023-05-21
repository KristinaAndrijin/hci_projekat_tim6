using HCI_Projekat.Model;
using HCI_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Service
{
    internal class UserService
    {
        public static User? Login(string username, string password)
        {
            using(var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(p => p.Email == username && p.Password == password);
                if(user != null)
                {
                    return user;
                }
                return null;
            }
        }
        public static User? Register(string username, string password, string name, string lastname, string roleString)
        {
            Enum.TryParse(roleString, out Role role);
            User newUser = new User() { 
                Email = username, 
                Password = password, 
                Name = name, 
                Lastname = lastname,
                Role = role };
            using(var context = new UserContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            return newUser;
        }
    }
}
