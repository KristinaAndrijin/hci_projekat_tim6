﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<Attraction>? Attractions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Restaurant>? Restaurants { get; set; }
        [JsonIgnore]
        public virtual ICollection<Accomodation>? Accomodations { get; set; } 
    }
}
