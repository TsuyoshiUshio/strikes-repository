using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrikesLibrary;

namespace DatabaseSeed
{

    public class SearchService
    {
        private ISearchRepository _repository;
        public SearchService(ISearchRepository repository)
        {
            _repository = repository;
        }

        public Task CreateIndexWithIndexerAsync()
        {
            return _repository.CreateIndexWithCosmosIndexerAsync();
        }

        public Task DeleteIndexAsync()
        {
            return _repository.DeleteIndexAsync();
        }
    }
}
