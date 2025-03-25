using System.Text.Json.Serialization;

namespace JOIEnergy.Domain.Models
{
    public class ElectricityReading
    {
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        [JsonPropertyName("reading")]
        public Decimal Reading { get; set; }
    }
}
