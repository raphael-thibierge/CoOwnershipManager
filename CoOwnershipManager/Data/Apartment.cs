using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoOwnershipManager.Data
{
    public class Apartment
    {
        public Apartment()
        {
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }

        public int BuildingId { get; set; }
        
        
        [JsonIgnore]
        public Building Building { get; set; }
        
        
        public ICollection<ApplicationUser> Unhabitants { get; set; }
    }
}
