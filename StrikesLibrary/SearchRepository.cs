using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrikesLibrary
{
    public interface ISearchRepository
    {
        Task CreateIndexWithCosmosIndexerAsync();
        Task DeleteIndexAsync();
        Task<IEnumerable<Package>> SearchAsync(string query);
    }

    public class SearchRepository : ISearchRepository
    {
        private AzureSearchServiceContext _context;

        public SearchRepository(AzureSearchServiceContext context)
        {
            _context = context;
        }

        public Task CreateIndexWithCosmosIndexerAsync()
        {
            return _context.CreateIndexWithCosmosIndexerAsync();
        }


        public Task DeleteIndexAsync()
        {
            return _context.DeleteIndexAsync();
        }

        public Task<IEnumerable<Package>> SearchAsync(string query)
        {
            return _context.SearchAsync(query);
        }
    }
}
