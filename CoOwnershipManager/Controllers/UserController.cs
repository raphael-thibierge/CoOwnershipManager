using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<ApplicationUser> getAppUser()
        {
            return _userManager.GetUserAsync(User).Result;
        }
        
        
        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> GetApartments()
        {
            return await _context.Users
                .Include(u => u.Apartment)
                .Include(u => u.Apartment.Unhabitants)
                .Include(u => u.Apartment.Building.Apartments)
                .Include(u => u.Apartment.Building.Address)
                .Include(u => u.Apartment.Building.Posts)
                .FirstAsync(u => u.Id == _userManager.GetUserId(User));
            
        }

        // GET: api/User
        [HttpGet("Apartment")]
        public async Task<ActionResult<Apartment>> GetUserApartment()
        {
            int resultApartmentId = _userManager.GetUserAsync(User).Result.ApartmentId;
            var apartment = await _context.Apartments.FirstAsync(a => a.Id==resultApartmentId);

            return apartment;

        }
        
        
    }
}