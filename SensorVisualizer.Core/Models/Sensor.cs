namespace SensorVisualizer.Core.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Temperature { get; set; }
    }
}
