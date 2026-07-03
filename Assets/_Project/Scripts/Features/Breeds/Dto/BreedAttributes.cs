using Newtonsoft.Json;

namespace TestTask.Features.Breeds.Dto
{
    public sealed class BreedAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
