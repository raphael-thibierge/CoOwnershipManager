using System;
using System.Collections.Generic;

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
        public Building Building { get; set; }

        public virtual ICollection<ApplicationUser> Unhabitants { get; set; }
    }
}
