using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JOIEnergy.Domain.Models
{
    public class SmartMeterReadings
    {
        [JsonPropertyName("smartMeterId")]
        public string SmartMeterId { get; set; }
        [JsonPropertyName("electricityReadings")]
        public List<ElectricityReading> ElectricityReadings { get; set; }
    }
}
