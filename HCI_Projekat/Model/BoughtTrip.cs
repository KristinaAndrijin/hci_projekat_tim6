using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Projekat.Model
{
    internal class BoughtTrip
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public Trip Trip { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
    }
}
