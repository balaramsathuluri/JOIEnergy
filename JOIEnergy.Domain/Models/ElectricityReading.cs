using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
