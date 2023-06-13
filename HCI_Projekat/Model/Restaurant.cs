using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public RestaurantType Type { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public bool IsDeleted { get; set; }
    }
    public enum RestaurantType
    {
        Ethno, Modern, FastFood
    }
}
