using SensorVisualizer.Core.Clients.OpenWeatherMap.Models;
using SensorVisualizer.Core.Models;

namespace SensorVisualizer.WPF.Models
{
    public class SensorViewModel
    {
        public Sensor Sensor { get; }
        public CurrentWeather WeatherInfo { get; }

        public SensorViewModel(Sensor sensor, CurrentWeather weatherInfo)
        {
            Sensor = sensor;
            WeatherInfo = weatherInfo;
        }
    }
}
