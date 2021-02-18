using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Authorization
{
    public class ApartmentMemberRequirement : IAuthorizationRequirement { }

    public class ApartmentMemberAuthorizationHandler:
        AuthorizationHandler<ApartmentMemberRequirement, Apartment>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApartmentMemberAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, ApartmentMemberRequirement requirement,
            Apartment resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            
            if (appUser.ApartmentId == resource.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}