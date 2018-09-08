using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrikesLibrary;

namespace StrikesRepository
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchPackage>> SearchNameAsync(string name);
    }
    public class SearchService :ISearchService
    {
        private ISearchRepository _repository;
        public SearchService(ISearchRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SearchPackage>> SearchNameAsync(string name)
        {
            var searchFields = new List<string>();
            searchFields.Add("name");
            return _repository.SearchAsync(name, searchFields);
        }
    }
}
