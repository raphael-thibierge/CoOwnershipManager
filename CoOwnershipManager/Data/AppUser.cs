using System;
namespace CoOwnershipManager.Data
{
    public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public AppUser(): base()
        {
        }

        public string FirstName { get; set;}
        public string LastName { get; set;}
    }
}
