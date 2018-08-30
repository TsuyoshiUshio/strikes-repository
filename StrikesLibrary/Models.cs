using System;

namespace StrikesLibrary
{
    public class Package
    {
        public string id { get; set; }
        public string Name { get; set;}
        public string Description { get; set; }
        public string Author { get; set; }
        public string ProjectPage { get; set; }
        public string ProjectRepo { get; set; }
        public DateTime CreatedTime { get; set; }
        public Release[] Releases { get; set; }

        public string NameIndex0 { get; set; }
        internal void Setup()
        {
            this.id = this.Name;
            if (!string.IsNullOrEmpty(Name))
            {
                this.NameIndex0 = this.Name.Substring(0, 1);
            }
        }
    }

    public enum ProviderType
    {
        Terraform,
        ARM, 
    }

    public class Release
    {
        public string Version { get; set; }
        public string ReleaseNote { get; set; }
        public ProviderType ProviderType { get; set; }

        public DateTime CreatedTime { get; set; }
    }

    public class RepositoryContext
    {
        public string ServerBaseURL { get; set; }
    }

    public enum PowerPlantStatus
    {
        Started, 
        Failed,
        Completed,
    }

    public class PowerPlant
    {
        // PartitionKey
        public string DeploymentName { get; set; }
        // RowKey
        public string PackageName { get; set; }
        public string Version { get; set; }
        public string ResourceGroup { get; set; }
        public string FunctionAppName { get; set; }
        public PowerPlantStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }


}
