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
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public BuildingController(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        
        // GET: api/Building
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Building>>> GetBuildings(int? addressId)
        {
            var query = _context.Buildings
                .Include(b=>b.Address)
                .Include(b=>b.Apartments); // could represent a leak of information... but it's a prototype..
            
            if (addressId != null)
                return await query.Where(b => b.AddressId == addressId).ToListAsync();

            return await query.ToListAsync();
        }

        // GET: api/Building/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Building>> GetBuilding(int id)
        {
            
            var building = await _context.Buildings
                .Include(b => b.Address)
                .Include(b => b.Apartments)
                .ThenInclude(a => a.Unhabitants)
                .Include(b => b.Posts)
                .ThenInclude(p => p.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (building == null)
                return NotFound();

            // check authorization, and return all data if is a building member
            var authorization =
                await _authorizationService.AuthorizeAsync(User, building, new BuildingMemberRequirement());
            if (authorization.Succeeded)
                return building;
            

            // else , return only few data
            // TODO : use DTO model
            return new Building()
            {
                Id = building.Id,
                Name = building.Name,
                Address = building.Address,
                Apartments = building.Apartments.Select(a => new Apartment()
                {
                    Number = a.Number,
                    Description = a.Description,
                    BuildingId = a.BuildingId,
                    Id = a.Id
                }).ToList()
            };
        }

        /*
        // PUT: api/Building/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuilding(int id, Building building)
        {
            if (id != building.Id)
            {
                return BadRequest();
            }

            _context.Entry(building).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildingExists(id))
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

        // POST: api/Building
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Building>> PostBuilding(Building building)
        {
            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuilding", new { id = building.Id }, building);
        }

        /*
        // DELETE: api/Building/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuilding(int id)
        {
            var building = await _context.Buildings.FindAsync(id);
            if (building == null)
            {
                return NotFound();
            }

            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        private bool BuildingExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
        
        */
    }
}
