using StrikesLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace DatabaseSeed
{
    class PackageFixture
    {
        /// <summary>
        /// Generate 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Package> GenerateTestFixture()
        {
            var packages = new List<Package>();
            packages.AddGeneratedPackage(
                    "hello-world",
                    "Hello world package for Strikes",
                    "Tsuyoshi Ushio",
                    "https://github.com/TsuoyshiUshio/StrikesHelloWorld",
                    "https://github.com/TsuoyshiUshio/StrikesHelloWorld",
                    "1.0.0",
                    "Initial version of the strikes");

            packages.AddGeneratedPackage(
                "foo-bar",
                "foo-bar package for Strikes",
                "Ry Cooder",
                "https://github.com/TsuoyshiUshio/StrikesFooBar",
                "https://github.com/TsuoyshiUshio/StrikesFooBar",
                "1.0.0",
                "Initial version of the strikes");
            return packages;
        }

    }

    public static class ListExtensions
    {
        public static void AddGeneratedPackage(this List<Package> packages,
            string name,
            string description,
            string author,
            string projectPage,
            string projectRepo,
            string version,
            string releaseNote)
        {
            packages.Add(createPackage(
                name,
                description,
                author,
                projectPage,
                projectRepo,
                version,
                releaseNote
            ));
        }



        private static Package createPackage(
            string name,
            string description,
            string author,
            string projectPage,
            string projectRepo,
            string version,
            string releaseNote
        )
        {
            var createdTime = DateTime.Now;
            return new Package()
            {
                Name = name,
                Description = description,
                Author = author,
                ProjectPage = projectPage,
                ProjectRepo = projectRepo,
                CreatedTime = createdTime,
                Releases = new Release[]
                {
                    new Release()
                    {
                        Version = version,
                        ReleaseNote = releaseNote,
                        ProviderType = ProviderType.Terraform,
                        CreatedTime = createdTime
                    }
                }
            };

        }
    }
}
