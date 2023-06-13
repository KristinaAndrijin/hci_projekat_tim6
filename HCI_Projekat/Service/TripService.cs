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
    internal class TripService
    {

        public static Trip? Add(string startAddress, string endAddress, string name, int price)
        {
            Trip newTrip = new Trip()
            {
                StartingAddress = startAddress,
                EndingAddress = endAddress,
                Name = name,
                Price = price
            };
            using (var context = new AppContext())
            {
                context.Trips.Add(newTrip);
                context.SaveChanges();
            }
            return newTrip;
        }

        public static Trip? Edit(int id, string startAddress, string endAddress, string name, int price)
        {
            using (var context = new AppContext())
            {
                var trip = context.Trips.Find(id);
                if (trip != null)
                {
                    trip.StartingAddress = startAddress;
                    trip.StartingAddress = endAddress;
                    trip.Name = name;
                    trip.Price = price;
                    context.SaveChanges();
                    return trip;
                }
                return null;
            }
        }

        public static List<Trip> GetAllAccomodations()
        {
            using (var context = new AppContext())
            {
                return context.Trips.Where(a => !a.IsDeleted).ToList();
            }
        }
    }
}
