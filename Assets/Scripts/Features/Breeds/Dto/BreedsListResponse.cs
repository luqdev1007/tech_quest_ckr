using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestTask.Features.Breeds.Dto
{
    public sealed class BreedsListResponse
    {
        [JsonProperty("data")]
        public List<BreedData> Data { get; set; }
    }
}
