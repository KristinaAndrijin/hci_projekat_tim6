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
        internal class RestaurantService
        {
            public static Restaurant? Add(string address, RestaurantType type, string name)
            {
                Restaurant newRestaurant = new Restaurant()
                {
                    Address = address,
                    Type = type,
                    Name = name
                };
                using (var context = new AppContext())
                {
                    context.Restaurants?.Add(newRestaurant);
                    context.SaveChanges();
                }
                return newRestaurant;
            }

            public static Restaurant Edit(int id, string address, RestaurantType type, string name)
            {
                using (var context = new AppContext())
                {
                    var restaurant = context.Restaurants?.Find(id);
                    if (restaurant != null)
                    {
                        restaurant.Address = address;
                        restaurant.Type = type;
                        restaurant.Name = name;
                        context.SaveChanges();
                        return restaurant;
                    }
                    return null;
                }
            }

        public static List<Restaurant> GetAllRestaurants()
        {
            using (var context = new AppContext())
            {
                return context.Restaurants.ToList();
            }
        }

        public static Restaurant FindById(int id)
        {
            using (var context = new AppContext())
            {
                var restaurant = context.Restaurants?.Find(id);
                if (restaurant != null)
                {
                    return restaurant;
                }
                return null;
            }
        }
    }
}
