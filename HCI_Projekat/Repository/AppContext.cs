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
    }
}
