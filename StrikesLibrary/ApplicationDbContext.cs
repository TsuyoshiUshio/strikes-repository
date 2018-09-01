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
        Package GetPackage(string name);
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

        // This part should be tested by integration testing.
        public Package GetPackage(string name)
        {
            var query = client.CreateDocumentQuery<Package>(
                    UriFactory.CreateDocumentCollectionUri(this.databaseId, typeof(Package).Name))
                .Where(p => p.Id == name);
            return query.FirstOrDefault<Package>();
        }



    }
}
