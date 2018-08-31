using System;
using System.Collections.Generic;
using System.Text;

namespace StrikesLibrary
{
    public interface IPackageRepository
    {
        Package GetPackage(string name);
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
    }
}
