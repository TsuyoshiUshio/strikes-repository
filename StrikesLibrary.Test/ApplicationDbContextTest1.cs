using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StrikesLibrary;
using System.Linq;

namespace StrikesLibrary.Test
{
    [TestClass]
    public class ApplicationDbContextTest1
    {
        [TestMethod]
        public void TestAppplicationDbContext()
        {
            IDocumentClient client = new DocumentClient(new System.Uri(CosmosDBTestHelper.COSMOSDB_EMULATOR_URI), CosmosDBTestHelper.COSMOSDB_EMULATOR_KEY);
            var context = new ApplicationDbContext(client, "database");
            var result = context.GetPackages("abcd", (p) => {
                return new Package[]
                {
                    new Package
                    {
                        Name = "ab",
                        NameIndex0 = "a"
                    },
                    new Package
                    {
                        Name = "abc",
                        NameIndex0 = "a"
                    },
                    new Package
                    {
                        Name = "abcd",
                        NameIndex0 = "a"
                    },
                    new Package
                    {
                        Name = "abcde",
                        NameIndex0= "a"
                    }
                };

            }).ToList();
            Assert.AreEqual("abcd", result[0].Name);
            Assert.AreEqual("abcde", result[1].Name);   
            
        }
    }
}
