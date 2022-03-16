using SensorVisualizer.Core.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SensorVisualizer.WPF.Converters
{
    public class SensorTemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Sensor sensor
                ? $"{sensor.Temperature.ToString(CultureInfo.InvariantCulture)} °C"
                : throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SensorLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is MapPoint sensor ? $"({sensor.Longitude:F}, {sensor.Latitude:F})" : throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
