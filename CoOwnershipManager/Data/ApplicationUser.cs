using System;
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
        public bool IsAdmin { get; set; }

        public static explicit operator ApplicationUser(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
