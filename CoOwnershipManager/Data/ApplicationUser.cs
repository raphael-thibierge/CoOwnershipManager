using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Data
{
    [DataContract]
    public sealed class ApplicationUser : IdentityUser
    {
        public ApplicationUser(): base()
        {
        }
        
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
    
        public string Name => String.Concat(FirstName, " ", LastName);
        
        public bool IsSuperAdmin { get; set; }
        
        public int? ApartmentId { get; set; }
        
        public Apartment Apartment { get; set; }
        
        
        public ICollection<Post> Posts { get; set; }

        public static explicit operator ApplicationUser(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
