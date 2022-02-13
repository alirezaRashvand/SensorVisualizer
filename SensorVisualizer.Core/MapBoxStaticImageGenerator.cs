using System;
using System.Globalization;

namespace SensorVisualizer.Core
{
    public static class MapBoxStaticImageGenerator
    {
        private const string UrlFormat = "https://api.mapbox.com/styles/v1/mapbox/streets-v11/static/pin-s+ff0000({Long},{Lat})/{Long},{Lat},{Zoom},0/{Width}x{Height}@2x?access_token={AccessToken}";
        public static Uri GenerateStaticImageUri(decimal longitude, decimal latitude, float zoom, int width, int height)
        {
            string url = UrlFormat
                .Replace("{AccessToken}", AppConsts.MapBoxAccessToken)
                .Replace("{Long}", longitude.ToString(CultureInfo.InvariantCulture))
                .Replace("{Lat}", latitude.ToString(CultureInfo.InvariantCulture))
                .Replace("{Zoom}", zoom.ToString(CultureInfo.InvariantCulture))
                .Replace("{Width}", width.ToString(CultureInfo.InvariantCulture))
                .Replace("{Height}", height.ToString(CultureInfo.InvariantCulture));
            return new Uri(url);
        }
    }
}
