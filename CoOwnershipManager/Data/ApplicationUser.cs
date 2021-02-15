using System;
namespace CoOwnershipManager.Data
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public ApplicationUser(): base()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
