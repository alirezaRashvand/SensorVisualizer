using System.Text.Json.Serialization;

namespace SensorVisualizer.Core.Clients.OpenWeatherMap.Models
{
    public class Weather
    {
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string ShortInfo { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
