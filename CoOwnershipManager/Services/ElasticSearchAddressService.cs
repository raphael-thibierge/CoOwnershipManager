// Source :
// https://github.com/enricobencivenga/ProductElasticSearch/blob/master/Services/ElasticSearchProductService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoOwnershipManager.Data;
using Microsoft.Extensions.Logging;
using Nest;

namespace CoOwnershipManager.Services
{
    public class ElasticSearchAddressService : IAddressService
    {
        private List<Address> _cache = new List<Address>();

        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;

        public ElasticSearchAddressService(IElasticClient elasticClient, ILogger<ElasticSearchAddressService> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }
       

        public async Task DeleteAsync(Address address)
        {
            await _elasticClient.DeleteAsync<Address>(address);

            if (_cache.Contains(address))
            {
                _cache.Remove(address);
            }
        }

        public async Task SaveSingleAsync(Address address)
        {
            if (_cache.Any(p => p.Id == address.Id))
            {
                await _elasticClient.UpdateAsync<Address>(address, u => u.Doc(address));
            }
            else
            {
                _cache.Add(address);
                await _elasticClient.IndexDocumentAsync<Address>(address);
            }
        }

        public async Task SaveManyAsync(Address[] addresses)
        {
            _cache.AddRange(addresses);
            var result = await _elasticClient.IndexManyAsync(addresses);
            if (result.Errors)
            {
                // the response can be inspected for errors
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                }
            }
        }

        public async Task SaveBulkAsync(Address[] addresses)
        {
            _cache.AddRange(addresses);        // TODO : WARNING HARDCODED INDEX
            var result = await _elasticClient.BulkAsync(b => b.Index("addresses").IndexMany(addresses));
            if (result.Errors)
            {
                // the response can be inspected for errors
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                }
            }
        }
    }
}
