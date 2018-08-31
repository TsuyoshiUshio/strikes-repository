using System;

namespace StrikesLibrary
{
    public class Package
    {
        private string _id;
        private string _name;

        public string id
        {
            get => _id;

            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _id = value;
            }
        }

        public string Description { get; set; }
        public string Author { get; set; }
        public string ProjectPage { get; set; }
        public string ProjectRepo { get; set; }
        public DateTime CreatedTime { get; set; }
        public Release[] Releases { get; set; }

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
