using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
namespace StrikesLibrary.Test
{
    public class UniqueKeyPolicyExtensionTest
    {
        [Fact]
        public void UniqKey_Add_Normal_Case()
        {
            var ExpectedFirstKey = "/foo";
            var ExpectedSecondKey = "/bar";
            var NotExpectedThriedKey = "/baz";

            var policy = new UniqueKeyPolicy();
            policy.AddUniqueKey(ExpectedFirstKey);
            policy.AddUniqueKey(ExpectedSecondKey);

            var keys = policy.UniqueKeys.ToList();
            Assert.Equal(ExpectedFirstKey, keys[0].Paths[0]);
            Assert.Equal(ExpectedSecondKey, keys[1].Paths[0]);
            Assert.Equal(2, keys.Count());
        }

        private UniqueKey getUniqueKey(string path)
        {
            var key = new UniqueKey();
            key.Paths.Add(path);
            return key;
        }
            
    }
}
