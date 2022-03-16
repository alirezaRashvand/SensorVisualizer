namespace SensorVisualizer.Core
{
    public static class AppConsts
    {
        public const string OpenWeatherMapApiKey = "7099db3ebff0dd6bcedcde66800c0703";
        public const string MapBoxAccessToken = "pk.eyJ1IjoiYWxpcmV6YXIiLCJhIjoiY2t5cHhxaWxkMGVtYzJ3cHR3djFua3ZpbyJ9.3VWHemlWdiSLEuYOres5IQ";

        public const int maxZoomLevel = 18;
        public const int minZoomLevel = 10;

        public const int SensorsPageRows = 2;
        public const int SensorsPageCols = 2;
        public const int SensorsPageSize = SensorsPageRows * SensorsPageCols;

        public const int MaxCustomPoints = 4;
        public const int CustomPointsPageRows = 2;
        public const int CustomPointsPageCols = 2;
    }
}
