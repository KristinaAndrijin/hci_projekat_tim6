using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? StartingAddress  { get; set; }
        public string? EndingAddress { get; set; }
        public int Price { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Attraction>? Attractions { get; set; } 
        public virtual ICollection<Restaurant>? Restaurants { get; set; } 
        public virtual ICollection<Accomodation>? Accomodations { get; set; } 

        public int AttractionsCount => Attractions?.Count ?? 0;
        public int RestaurantsCount => Restaurants?.Count ?? 0;
        public int AccomodationsCount => Accomodations?.Count ?? 0;
    }
}
