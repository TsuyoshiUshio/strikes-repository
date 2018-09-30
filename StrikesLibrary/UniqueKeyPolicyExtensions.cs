using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrikesLibrary
{
    public static class UniqueKeyPolicyExtensions
    {
        public static void AddUniqueKey(this UniqueKeyPolicy policy, string path)
        {
            var keys = policy.UniqueKeys;
            var uniqueKey = new UniqueKey();
            var paths = uniqueKey.Paths;
            paths.Add(path);
            policy.UniqueKeys.Add(uniqueKey);
        }
    }
}
