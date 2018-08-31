using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using StrikesLibrary;

namespace StrikesLibrary.Test
{
    [TestClass]
    public class PackageTest
    {
        [TestMethod]
        public void TestPackageSetup()
        {
            var p = new Package
            {
                Name = "abcd"
            };
            
            Assert.AreEqual("abcd", p.id);
        }

    }
}
