using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    public class Accomodation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public AccomodationType Type { get; set; }
        public ICollection<Trip> Trips { get; set; }
        public bool IsDeleted { get; set; }
    }
    public enum AccomodationType
    {
        Hotel, Motel, Tent
    }
}
