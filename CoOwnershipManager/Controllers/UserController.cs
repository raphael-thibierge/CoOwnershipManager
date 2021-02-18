using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;

namespace CoOwnershipManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        // GET: api/User/Building
        // Redirect to user's building API endpoint if exists
        // TODO improve this...
        [Authorize]
        [HttpGet("Building")]
        public async Task<ActionResult> GetUserBuilding()
        {

            var userId = _userManager.GetUserId(User);

            
            /* LINQ just to play around , seems very un-efficient
            
            very un-efficient way to get user's building, 
            it was done like this only to try LINQ syntax
            (will be removed...)
            
             TODO : study Warning compilation
            Microsoft.EntityFrameworkCore.Query[20504]
            Compiling a query which loads related collections for more than one collection navigation either
            via 'Include' or through projection but no 'QuerySplittingBehavior' has been configured.
            By default Entity Framework will use 'QuerySplittingBehavior.SingleQuery' which can potentially 
            result in slow query performance. See https://go.microsoft.com/fwlink/?linkid=2134277 for more information.
            To identify the query that's triggering this warning call
             'ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))'
             */
            
            var userApartment = await (
                from applicationUser in _context.Users
                join apartment in _context.Apartments on applicationUser.ApartmentId equals apartment.Id
                where applicationUser.Id == userId
                select apartment
                ).FirstAsync();

            if (userApartment == null)
                return NotFound();

            return RedirectToAction("GetBuilding", "Building", new {Id=userApartment.BuildingId});
        }

        // GET: api/User
        // Redirect GetApartment API endpoint id user has one else return NotFound 
        [Authorize]
        [HttpGet("Apartment")]
        public async Task<ActionResult> GetUserApartment()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            return currentUser.ApartmentId == null
                ? NotFound()
                : RedirectToAction("GetApartment", "Apartment", new {Id = currentUser.ApartmentId});
        }
    }
}