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

            p.Setup();

            Assert.AreEqual("abcd", p.id);
            Assert.AreEqual("a", p.NameIndex0);
        }

    }
}
