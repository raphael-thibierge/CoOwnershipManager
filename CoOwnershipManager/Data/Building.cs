using System;
using System.Collections.Generic;

namespace CoOwnershipManager.Data
{
    public class Building
    {
        public Building()
        {
            this.Apartments = new HashSet<Apartment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string StreetName { get; set; }
        public int StreetNumber { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }
    }
}
