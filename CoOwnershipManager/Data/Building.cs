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

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
