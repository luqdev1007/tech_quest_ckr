using Newtonsoft.Json;

namespace TestTask.Features.Breeds.Dto
{
    public sealed class BreedResponse
    {
        [JsonProperty("data")]
        public BreedData Data { get; set; }
    }
}
