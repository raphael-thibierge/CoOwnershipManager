using System;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Authorization
{
    public class UserIsAdminRequirements : IAuthorizationRequirement { }

    public class UserIsAdminAuthorizationHandler: AuthorizationHandler<UserIsAdminRequirements>
    {

        UserManager<ApplicationUser> _userManager;
        ApplicationDbContext _dbContext;

        public UserIsAdminAuthorizationHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserIsAdminRequirements requirement)
        {

            // TODO : Find a way to get ApplicationUser from context without a DB query (if possible)
            var user = _dbContext.Users.Find(_userManager.GetUserId(context.User));
            
            if (user == null)
            {
                context.Fail();
            }
            else if (user.IsAdmin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
