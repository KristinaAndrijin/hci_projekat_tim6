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
    internal class AttractionService
    {
        //public static bool HasLoggedIn { get; set; }
        //public static User? CurrentlyLoggedIn { get; set; }
        //public static User? Login(string username, string password)
        //{
        //    using (var context = new AppContext())
        //    {
        //        var user = context.Users.FirstOrDefault(p => p.Email == username && p.Password == password);
        //        if (user != null)
        //        {
        //            HasLoggedIn = true;
        //            CurrentlyLoggedIn = user;
        //            return user;
        //        }
        //        return null;
        //    }
        //}
        public static Attraction? Add(string address, AttractionType type, string name)
        {
            Attraction newAttraction = new Attraction()
            {
                Address = address,
                Type = type,
                Name = name
            };
            using (var context = new AppContext())
            {
                context.Attractions.Add(newAttraction);
                context.SaveChanges();
            }
            return newAttraction;
        }

        public static Attraction? Edit(int id, string address, AttractionType type, string name)
        {
            using (var context = new AppContext())
            {
                var att = context.Attractions.Find(id);
                if (att != null)
                {
                    att.Address = address;
                    att.Type = type;
                    att.Name = name;
                    context.SaveChanges();
                    return att;
                }
                return null;
            }
        }


        //public static void Logout()
        //{
        //    HasLoggedIn = false;
        //    CurrentlyLoggedIn = null;
        //}
    }
}
