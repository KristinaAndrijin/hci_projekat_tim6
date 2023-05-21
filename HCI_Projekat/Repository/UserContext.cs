using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Repository
{
    internal class UserContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
    }
}
