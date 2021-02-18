using System;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Authorization
{
    public class UserIsAdminRequirements : IAuthorizationRequirement { }

    public class UserIsAdminAuthorizationHandler: AuthorizationHandler<UserIsAdminRequirements>
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserIsAdminAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, UserIsAdminRequirements requirement)
        {

            // TODO : Find a way to get ApplicationUser from context without a DB query (if possible)
            var user = await _userManager.GetUserAsync(context.User);

            // TODO : Find a way to avoid anonymous users in this handler
            if (user == null)
            {
                context.Fail();
            }
            else if (user.IsSuperAdmin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
