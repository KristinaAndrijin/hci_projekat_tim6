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

        public static Trip? Add(string startAddress, string endAddress, string name, int price, List<Accomodation> accomodations = null, List<Attraction> attractions = null, List<Restaurant> rastaurants = null)
        {
            if (accomodations == null)
            {
                accomodations=new List<Accomodation>();
            }
            if (attractions == null) { attractions=new List<Attraction>();}
            if (rastaurants == null) { rastaurants = new List<Restaurant>(); }
            Trip newTrip = new Trip()
            {
                StartingAddress = startAddress,
                EndingAddress = endAddress,
                Name = name,
                Price = price,
                Accomodations = accomodations,
                Restaurants = rastaurants,
                Attractions = attractions
            };
            using (var context = new AppContext())
            {
                context.Trips.Add(newTrip);
                context.SaveChanges();
            }
            return newTrip;
        }

        public static Trip? Edit(int id, string startAddress, string endAddress, string name, int price, List<Accomodation> accomodations = null, List<Attraction> attractions = null, List<Restaurant> rastaurants = null)
        {
            using (var context = new AppContext())
            {
                var trip = context.Trips.Find(id);

                if (trip != null)
                {
                    if (accomodations == null && trip.Accomodations != null)
                    {
                        accomodations = (List<Accomodation>?)trip.Accomodations;
                    } else if (accomodations == null && trip.Accomodations == null)
                        {
                            accomodations = new List<Accomodation>();
                        }
                    if (attractions == null && trip.Attractions != null)
                    {
                        attractions = (List<Attraction>?)trip.Attractions;
                    }
                    else if (attractions == null && trip.Accomodations == null)
                    {
                        attractions = new List<Attraction>();
                    }
                    if (rastaurants == null && trip.Restaurants != null)
                    {
                        rastaurants = (List<Restaurant>?)trip.Restaurants;
                    }
                    else if (rastaurants == null && trip.Restaurants == null)
                    {
                        rastaurants = new List<Restaurant>();
                    }
                    trip.StartingAddress = startAddress;
                    trip.StartingAddress = endAddress;
                    trip.Name = name;
                    trip.Price = price;
                    trip.Accomodations = accomodations;
                    trip.Restaurants = rastaurants;
                    trip.Attractions = attractions;
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

        public static Trip? FindById(int id)
        {
            using (var context = new AppContext())
            {
                var trip = context.Trips.Find(id);

                if (trip != null)
                {
                    return trip;
                }
                return null;
            }
        }
    }
}
