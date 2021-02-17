using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoOwnershipManager.Data
{
    public class Address
    {
        public Address()
        {
        }

        public int Id { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        [JsonIgnore]
        public virtual Building Building { get; set; }
      
    }
}
