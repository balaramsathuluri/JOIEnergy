using System.Text.Json.Serialization;

namespace JOIEnergy.Domain.Models
{
    public class MeterReadings
    {
        [JsonPropertyName("smartMeterId")]
        public string SmartMeterId { get; set; }

        [JsonPropertyName("electricityReadings")]
        public List<ElectricityReading> ElectricityReadings { get; set; }
    }
}
