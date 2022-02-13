using System.Text.Json.Serialization;

namespace SensorVisualizer.Core.Clients.OpenWeatherMap.Models
{
    public class Temperature
    {
        public float Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public float TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public float TempMax { get; set; }

        public float Pressure { get; set; }
    }
}
