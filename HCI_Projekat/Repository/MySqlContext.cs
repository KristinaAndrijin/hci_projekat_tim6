using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // for DbContextOptionsBuilder
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // for ServerVersion


namespace HCI_Projekat.Repository
{
    public class MySqlContext : Microsoft.EntityFrameworkCore.DbContext
    {
        static readonly string connectionString = "Server=localhost; User ID=root; Password=root; Database=hcisql";
        public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<User> SomeMoreUsers { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        
    }
}
