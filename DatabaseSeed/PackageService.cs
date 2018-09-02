using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StrikesLibrary;

namespace DatabaseSeed
{
    public class PackageService
    {
        private IPackageRepository _packageRepository;

        public PackageService(IPackageRepository repository)
        {
            this._packageRepository = repository;
        }

        public async Task InitializeAsync()
        {
            // Delete Database 
            await _packageRepository.DeleteDatabaseIfExistsAsync();
            // Create Database
            await _packageRepository.CreateDatabaseIfNotExistsAsync();
            // Create Collection
            await _packageRepository.CreatePackageCollectionAsync();
        }

        public async Task CreatePackageAsync(Package p)
        {
           await _packageRepository.CreatePackageAsync(p);
        }
    }
}
