using Newtonsoft.Json;

namespace TestTask.Features.Breeds.Dto
{
    public sealed class BreedData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public BreedAttributes Attributes { get; set; }
    }
}
