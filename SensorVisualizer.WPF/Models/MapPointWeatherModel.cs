using SensorVisualizer.Core.Clients.OpenWeatherMap.Models;
using SensorVisualizer.Core.Models;

namespace SensorVisualizer.WPF.Models
{
    public class MapPointWeatherModel
    {
        public MapPoint MapPoint { get; set; }
        public CurrentWeather Weather { get; set; }
    }
}
