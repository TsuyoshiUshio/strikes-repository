using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace StrikesLibrary
{
    public interface IValidatable
    {

    }
    public class Package : IValidatable
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [Url]
        public string ProjectPage { get; set; }
        [Url]
        public string ProjectRepo { get; set; }
        public DateTime CreatedTime { get; set; }
        public Release[] Releases { get; set; }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
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
