using System.Text.Json.Serialization;

namespace SensorVisualizer.Core.Clients.OpenWeatherMap.Models
{
    public class CurrentWeather
    {
        [JsonPropertyName("coord")]
        public Coordinate Coordinate { get; set; }

        public Weather[] Weather { get; set; }

        [JsonPropertyName("main")]
        public Temperature Temperature { get; set; }

        [JsonPropertyName("name")]
        public string CityName { get; set; }
    }
}
