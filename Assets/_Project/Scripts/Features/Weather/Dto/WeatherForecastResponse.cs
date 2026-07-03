using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestTask.Features.Weather.Dto
{
    public sealed class WeatherForecastResponse
    {
        [JsonProperty("properties")]
        public WeatherForecastProperties Properties { get; set; }
    }

    public sealed class WeatherForecastProperties
    {
        [JsonProperty("periods")]
        public List<WeatherForecastPeriod> Periods { get; set; }
    }

    public sealed class WeatherForecastPeriod
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("temperatureUnit")]
        public string TemperatureUnit { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
