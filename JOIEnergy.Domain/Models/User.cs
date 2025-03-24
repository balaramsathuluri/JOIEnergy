using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JOIEnergy.Domain.Models
{
    public class User
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("smartMeterId")]
        public string SmartMeterId { get; set; }

        [JsonPropertyName("powerSupplier")]
        public string PowerSupplier { get; set; }
    }
}
