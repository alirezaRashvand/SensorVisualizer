using SensorVisualizer.Core.Clients.OpenWeatherMap.Models;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SensorVisualizer.Core.Clients.OpenWeatherMap
{
    public class OpenWeatherMapClient : IDisposable
    {
        private const string BaseAddressUrl = "https://api.openweathermap.org/data/2.5/";
        private const string WaetherInfoUrl = "weather";

        private const string ApiKeyParameterName = "appid";
        private const string UnitsParameterName = "units";
        private readonly HttpClient client;
        private readonly string ApiKey;

        public OpenWeatherMapClient(string apiKey)
        {
            client = new HttpClient() { BaseAddress = new Uri(BaseAddressUrl) };
            ApiKey = apiKey;
        }

        public void Dispose() => client.Dispose();

        public Task<CurrentWeather> CurrentWeatherAsync(decimal lat, decimal lon, OpenWeatherMapTemperatureUnit unit = OpenWeatherMapTemperatureUnit.Celsius)
        {
            NameValueCollection parms = new()
            {
                { "lat", lat.ToString(CultureInfo.InvariantCulture) },
                { "lon", lon.ToString(CultureInfo.InvariantCulture) },
                { UnitsParameterName, OpenWeatherMapTemperatureUnitToString(unit) }
            };
            return client.GetFromJsonAsync<CurrentWeather>(GenerateUri(WaetherInfoUrl, parms), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public Task<CurrentWeather> CurrentWeatherAsync(decimal lat, decimal lon, CancellationToken cancellationToken, OpenWeatherMapTemperatureUnit unit = OpenWeatherMapTemperatureUnit.Celsius)
        {
            NameValueCollection parms = new()
            {
                { "lat", lat.ToString(CultureInfo.InvariantCulture) },
                { "lon", lon.ToString(CultureInfo.InvariantCulture) },
                { UnitsParameterName, OpenWeatherMapTemperatureUnitToString(unit) }
            };
            return client.GetFromJsonAsync<CurrentWeather>(GenerateUri(WaetherInfoUrl, parms), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, cancellationToken);
        }

        private string GenerateUri(string path, NameValueCollection parameters = null)
        {
            NameValueCollection parms = HttpUtility.ParseQueryString($"{ApiKeyParameterName}={ApiKey}");
            if (parameters != null)
                parms.Add(parameters);
            return $"{path}?{parms}";
        }

        private static string OpenWeatherMapTemperatureUnitToString(OpenWeatherMapTemperatureUnit unit)
        {
            return unit switch
            {
                OpenWeatherMapTemperatureUnit.Fahrenheit => "imperial",
                OpenWeatherMapTemperatureUnit.Celsius => "metric",
                OpenWeatherMapTemperatureUnit.Kelvin => null,
                _ => throw new NotImplementedException(),
            };
        }
    }

    public enum OpenWeatherMapTemperatureUnit
    {
        Fahrenheit,
        Celsius,
        Kelvin
    }
}
