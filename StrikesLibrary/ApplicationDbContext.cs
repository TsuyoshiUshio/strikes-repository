using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using System.Text.RegularExpressions;

namespace StrikesLibrary
{

    public interface IApplicationDbContext
    {
        IEnumerable<Package> GetPackages(string name);
    }
    public class ApplicationDbContext : IApplicationDbContext
    {
        private IDocumentClient client;
        private string databaseId;
        public ApplicationDbContext(IDocumentClient client, string databaseId)
        {
            this.client = client;
            this.databaseId = databaseId;
        }

        // I can't unit testing for this method. 
        // Please test via Integration test
        /// <summary>
        /// GetPackages get Packages
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Package> GetPackages(string name)
        {
            return GetPackages(name, GetPackagesWithIndexQuery);
        }

        // This part should be tested by integration testing.  
        private IEnumerable<Package> GetPackagesWithIndexQuery(string nameIndex0)
        {
            var query = client.CreateDocumentQuery<Package>(
                UriFactory.CreateDocumentCollectionUri(this.databaseId, typeof(Package).Name));
            if (!string.IsNullOrEmpty(nameIndex0))
            {
                query.Where<Package>(p => p.NameIndex0 == nameIndex0);
            }
            return query.ToArray<Package>();
        }

        internal IEnumerable<Package> GetPackages(string name, Func<string, IEnumerable<Package>> query)
        {
            return query(name.FirstIndex()).Where(p => p.Name.StartsWith(name));         
        }


    }
}
