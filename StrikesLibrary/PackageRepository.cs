using System;
using System.Collections.Generic;
using System.Text;

namespace StrikesLibrary
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetPackages(string name);
    }
    public class PackageRepository : IPackageRepository
    {
        private readonly IApplicationDbContext dbContext;
        public PackageRepository(IApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<Package> GetPackages(string name)
        {
            return this.dbContext.GetPackages(name);
        }
    }
}
