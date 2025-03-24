using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JOIEnergy.Domain.Enums;

namespace JOIEnergy.Domain.Models
{
    public class PricePlan
    {
        [JsonPropertyName("planName")]
        public string PlanName { get; set; }

        [JsonPropertyName("energySupplier")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Supplier EnergySupplier { get; set; }

        [JsonPropertyName("unitRate")]
        public decimal UnitRate { get; set; }

        [JsonPropertyName("peakTimeMultiplier")]
        public IList<PeakTimeMultiplier> PeakTimeMultiplier { get; set; }

        public decimal GetPrice(DateTime datetime)
        {
            var multiplier = PeakTimeMultiplier.FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

            if (multiplier?.Multiplier != null)
            {
                return multiplier.Multiplier * UnitRate;
            }
            else
            {
                return UnitRate;
            }
        }
    }
    public class PeakTimeMultiplier
    {
        [JsonPropertyName("dayOfWeek")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DayOfWeek DayOfWeek { get; set; }
        [JsonPropertyName("multiplier")]
        public decimal Multiplier { get; set; }
    }
}
