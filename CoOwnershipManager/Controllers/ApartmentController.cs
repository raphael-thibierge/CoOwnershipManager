using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Authorization;

namespace CoOwnershipManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public ApartmentController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        /*
        // GET: api/Apartment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apartment>>> GetApartments()
        {
            return await _context.Apartments.ToListAsync();
        }
        */
        

        // GET: api/Apartment/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Apartment>> GetApartment(int id)
        {
            var apartment = await _context.Apartments
                .Include(a => a.Unhabitants)
                .Include(a => a.Building)
                .Include(a => a.Building.Address)
                .FirstOrDefaultAsync(a => a.Id== id);

            if (apartment == null) 
                return NotFound();

            // check if user has access to apartment before sending infos
            var check = await _authorizationService.AuthorizeAsync(User, apartment, new ApartmentMemberRequirement());
            if (!check.Succeeded)
                return Unauthorized();

            return apartment;
        }
        
        /*
         
        // PUT: api/Apartment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(int id, Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(apartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Apartment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apartment>> PostApartment(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment);
        }
        /*
        // DELETE: api/Apartment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            var apartment = await _context.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return NotFound();
            }

            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
        
        private bool ApartmentExists(int id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }
    }
}
