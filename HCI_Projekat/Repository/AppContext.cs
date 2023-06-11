using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCI_Projekat.Model;

namespace HCI_Projekat.Repository
{
    internal class AppContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Trip>? Trips { get; set; }
        public DbSet<Accomodation>? Accomodations { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<Attraction>? Attractions { get; set; }
        public DbSet<BoughtTrip>? BoughtTrips { get; set; }
    }
}
