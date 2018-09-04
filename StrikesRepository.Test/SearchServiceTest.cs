using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StrikesLibrary;
using Xunit;

namespace StrikesRepository.Test
{
    public class SearchServiceTest
    {
        [Fact]
        public async Task Search_name_normal_case()
        {
            var fixture = new ParameterFixture();
            var service = new SearchService(fixture.SearchRepoitory);
            var results = await service.SearchNameAsync("h*");
            Assert.Equal(fixture.Expected, results);
            fixture.VerifyCreated();
        }

        private class ParameterFixture
        {
            private Mock<ISearchRepository> _repositoryMock;

            public ISearchRepository SearchRepoitory => _repositoryMock.Object;

            public ParameterFixture()
            {
                _repositoryMock = new Mock<ISearchRepository>();
                SetUp();
            }

            public IEnumerable<SearchPackage> Expected => _results;

            private List<string> _searchFields;
            private List<SearchPackage> _results;
            private void SetUp()
            {
                _searchFields = new List<string>();
                _searchFields.Add("name");

                _results = new List<SearchPackage>()
                {
                    new SearchPackage()
                    {
                        Name = "hello-world"
                    }
                };

                _repositoryMock.Setup(r => r.SearchAsync("h*", _searchFields)).ReturnsAsync(_results);

            }

            public void VerifyCreated()
            {
                _repositoryMock.Verify(r => r.SearchAsync("h*", _searchFields));
            }
            
        }
    }
}
