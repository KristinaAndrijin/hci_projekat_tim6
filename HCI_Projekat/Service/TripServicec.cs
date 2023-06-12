using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AppContext = HCI_Projekat.Repository.AppContext;

namespace HCI_Projekat.Service
{
    internal class TripServicec
    {

        public static Trip? Add()
        {
            Trip newUser = new Trip()
            {
                Name = "blabka",
                StartingAddress = "uh",
                EndingAddress = "nes",
                Price = 513
            };
            using (var context = new AppContext())
            {
                context.Trips.Add(newUser);
                context.SaveChanges();
            }
            return newUser;
        }

    }
}
