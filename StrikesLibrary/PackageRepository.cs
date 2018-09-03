using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrikesLibrary
{
    public interface IPackageRepository
    {
        Package GetPackage(string name);
        Task DeleteDatabaseIfExistsAsync();
        Task CreateDatabaseIfNotExistsAsync();
        Task CreatePackageCollectionAsync();
        Task CreatePackageAsync(Package package);

    }
    public class PackageRepository : IPackageRepository
    {
        private readonly IApplicationDbContext dbContext;
        public PackageRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Package GetPackage(string name)
        {
            return this.dbContext.GetPackage(name);
        }

        public async Task DeleteDatabaseIfExistsAsync()
        {
            await dbContext.DeleteDatabaseIfExistsAsync();
        }

        public async Task CreateDatabaseIfNotExistsAsync()
        {
            await dbContext.CreateDatabaseIfNotExistsAsync();
        }

        public async Task CreatePackageCollectionAsync()
        {
            await dbContext.CreateDocumentCollectionIfNotExistsAsync<Package>();
        }

        public async Task CreatePackageAsync(Package package)
        {
            await dbContext.CreateDocumentAsync<Package>(package);
        }

    }
}
