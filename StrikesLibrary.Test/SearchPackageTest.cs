using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StrikesLibrary.Test
{

    public class SearchPackageTest

    {
        [Fact]
        public void Convert_To_Package()
        {
            var ExpectedId = "foo";
            var ExpectedName = "bar";
            var ExpectedDescription = "baz";
            var ExpectedAuthor = "tsuyoshi";
            var ExpectedProjectPage = "https://foo.bar";
            var ExpectedProjectRepo = "https://bar.baz";
            var ExpectedTime = DateTime.Now;
            var ExpectedIsDelete = true;

            var ReleasesString = "[\r\n  {\r\n    \"Version\": \"1.0.0\",\r\n    \"ReleaseNote\": \"Initial implementation for the first G.A.\",\r\n    \"ProviderType\": \"Terraform\",\r\n    \"CreatedTime\": \"2018-10-11T12:13:14Z\"\r\n  }\r\n]";


            var searchPackage = new SearchPackage
            {
                Id = ExpectedId,
                Name = ExpectedName,
                Description = ExpectedDescription,
                Author = ExpectedAuthor,
                ProjectPage = ExpectedProjectPage,
                ProjectRepo = ExpectedProjectRepo,
                CreatedTime = ExpectedTime,
                IsDeleted = ExpectedIsDelete,
                Releases = ReleasesString,
            };

            var p = searchPackage.ToPackage();
            Assert.Equal(ExpectedId, p.Id);
            Assert.Equal(ExpectedName, p.Name);
            Assert.Equal(ExpectedDescription, p.Description);
            Assert.Equal(ExpectedProjectPage, p.ProjectPage);
            Assert.Equal(ExpectedProjectRepo, p.ProjectRepo);
            Assert.Equal(ExpectedTime, p.CreatedTime);
            Assert.Equal(ExpectedIsDelete, p.IsDeleted);

            Assert.Equal("1.0.0", p.Releases[0].Version);
            Assert.Equal("Initial implementation for the first G.A.", p.Releases[0].ReleaseNote);
            Assert.Equal(ProviderType.Terraform, p.Releases[0].ProviderType);
            Assert.Equal(new DateTime(2018, 10, 11, 12, 13, 14), p.Releases[0].CreatedTime);


        }
    }

}
