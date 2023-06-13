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
            
            Trip newTrip = new Trip()
            {
                StartingAddress = startAddress,
                EndingAddress = endAddress,
                Name = name,
                Price = price,
                Accomodations = new List<Accomodation>(),
                Restaurants = new List<Restaurant>(),
                Attractions = new List<Attraction>()
            };
            
            using (var context = new AppContext())
            {
                if (accomodations != null)
                {
                    foreach (Accomodation accomodation in accomodations)
                    {
                        var obj = context.Accomodations.Find(accomodation.Id);
                        newTrip.Accomodations.Add(obj);
                        obj.Trips.Add(newTrip);
                        
                    }
                }
                if (attractions != null) 
                {
                    foreach (Attraction attraction in attractions)
                    {
                        var obj = context.Attractions.Find(attraction.Id);
                        newTrip.Attractions.Add(obj);
                        obj.Trips.Add(newTrip);
                    }
                }
                if (rastaurants != null) 
                {
                    foreach (Restaurant rastaurant in rastaurants)
                    {
                        var obj = context.Restaurants.Find(rastaurant.Id);
                        newTrip.Restaurants.Add(obj);
                        obj.Trips.Add(newTrip);
                    }
                }
                context.Trips.Add(newTrip);
                context.SaveChanges();
            }
            return newTrip;
        }

        public static Trip? Edit(int id, string startAddress, string endAddress, string name, int price, List<Accomodation> accomodations = null, List<Attraction> attractions = null, List<Restaurant> restaurants = null)
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
                    if(accomodations != null)
                    {
                        foreach(Accomodation accomodation in trip.Accomodations)
                        {
                            context.Accomodations.Find(accomodation.Id).Trips.Remove(context.Trips.Find(trip.Id));

                        }
                        trip.Accomodations = new List<Accomodation>();
                        foreach(Accomodation accomodation in accomodations)
                        {
                            var obj = context.Accomodations.Find(accomodation.Id);
                            trip.Accomodations.Add(obj);
                            obj.Trips.Add(trip);

                        }
                    }
                    if (restaurants != null)
                    {
                        foreach(Restaurant restaurant in trip.Restaurants)
                        {
                            context.Restaurants.Find(restaurant.Id).Trips.Remove(context.Trips.Find(trip.Id));
                        }

                        trip.Restaurants = new List<Restaurant>();
                        foreach (Restaurant restaurant in restaurants)
                        {
                            var obj = context.Restaurants.Find(restaurant.Id);
                            trip.Restaurants.Add(obj);
                            obj.Trips.Add(trip);

                        }
                    }
                    if (attractions != null)
                    {
                        foreach(Attraction attraction in trip.Attractions)
                        {
                            context.Attractions.Find(attraction.Id).Trips.Remove(context.Trips.Find(trip.Id));
                        }

                        trip.Attractions = new List<Attraction>();
                        foreach (Attraction attraction in attractions)
                        {
                            var obj = context.Attractions.Find(attraction.Id);
                            trip.Attractions.Add(obj);
                            obj.Trips.Add(trip);
                        }
                    }
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
