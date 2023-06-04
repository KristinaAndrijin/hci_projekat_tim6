using HCI_Projekat.Model;
using HCI_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AppContext = HCI_Projekat.Repository.AppContext;

namespace HCI_Projekat.Service
{
    internal class UserService
    {
        public static bool HasLoggedIn { get; set; }
        public static User? CurrentlyLoggedIn { get; set; }
        public static User? Login(string username, string password)
        {
            using(var context = new AppContext())
            {
                var user = context.Users.FirstOrDefault(p => p.Email == username && p.Password == password);
                if(user != null)
                {
                    HasLoggedIn = true;
                    CurrentlyLoggedIn = user;
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
            using(var context = new AppContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
            return newUser;
        }

        public static void Logout()
        {
            HasLoggedIn = false;
            CurrentlyLoggedIn = null;
        }
    }
}
