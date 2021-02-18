using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoOwnershipManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CoOwnershipManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.apartment = null;
            
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ApplicationUser appUser = (await _userManager.GetUserAsync(User));

                if (appUser.ApartmentId != null)
                {
                    Apartment apartment = _context.Apartments.First(a => a.Id == appUser.ApartmentId);
                    ViewBag.apartment = apartment;
                }

            }
            return View();
        }
        
        [HttpPost("/Join")]
        [Authorize]
        public async Task<IActionResult> Join(int apartmentId)
        {
            // check if apartment exists 
            var apartment = _context.Apartments.Find(apartmentId);
            if (apartment == null)
                return NotFound(); // todo : not sure it's the appropriate http status code
            
            // get user
            var currentUser = await _userManager.GetUserAsync(User);
            
            // set user's apartment 
            currentUser.Apartment = apartment;

            // save
            await _context.SaveChangesAsync();
            
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
