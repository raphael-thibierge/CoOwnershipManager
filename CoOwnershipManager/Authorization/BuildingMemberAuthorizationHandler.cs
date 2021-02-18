using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Authorization
{
    public class BuildingMemberRequirement : IAuthorizationRequirement { }

    public class BuildingMemberAuthorizationHandler:
        AuthorizationHandler<BuildingMemberRequirement, Building>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public BuildingMemberAuthorizationHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, BuildingMemberRequirement requirement,
            Building resource)
        {
            // TODO : may not be the good way to do that..
            var user = await _userManager.GetUserAsync(context.User);
            var apartment = _context.Apartments.Find(user.ApartmentId);
            if (apartment != null && apartment.BuildingId == resource.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}