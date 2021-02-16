using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;

namespace CoOwnershipManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly ILogger<SearchController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ElasticClient _elasticClient;

        public SearchController(ILogger<SearchController> logger,  ApplicationDbContext context, ElasticClient elasticClient)
        {
            _logger = logger;
            _context = context;
            _elasticClient = elasticClient;
        }

        [Route("Addresses")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses(string query, int page = 1, int pageSize = 8)
        {
            var response = await _elasticClient.SearchAsync<Address>
        (
                s => s.Query(q => q.QueryString(d => d.Query('*' + query + '*')))
                    .From((page - 1) * pageSize)
                    .Size(pageSize));

            if (!response.IsValid)
            {
                // We could handle errors here by checking response.OriginalException 
                //or response.ServerError properties
                _logger.LogError("Failed to search documents");
                // TODO
                //return View("Results", new Product[] { });
            }

            

            if (response.IsValid && response.Total > page * pageSize)
            {
                // TODO
                //ViewData["next"] = GetSearchUrl(query, page + 1, pageSize);
            }
                
            return response.Documents.ToArray();
            //return View("Results", response.Documents);
        }

      

        //   GET: api/Search/Elastic
        // Only for development purpose
        [HttpGet("Elastic")]
        public async Task<IActionResult> Elastic()
        {
            await _elasticClient.DeleteByQueryAsync<Address>(q => q.MatchAll());

            var allProducts = (await _context.Addresses.ToListAsync()).ToArray();
            _logger.LogDebug("Search retrieve all adresses");
            
            await _elasticClient.IndexManyAsync(allProducts);

            _logger.LogDebug("Search retrieve all adresses");

            return Ok($"{allProducts.Length} address(s) reindexed");
        }


    }
}