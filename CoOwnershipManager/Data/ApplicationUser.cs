using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(): base()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSuperAdmin { get; set; }

        public int? ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public static explicit operator ApplicationUser(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
