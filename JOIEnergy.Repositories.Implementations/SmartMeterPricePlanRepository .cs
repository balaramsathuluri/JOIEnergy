using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Repository.Implementations.Utilities;
using JOIEnergy.Repository.Interfaces;

namespace JOIEnergy.Repository.Implementations
{
    public class SmartMeterPricePlanRepository : ISmartMeterPricePlanRepository
    {
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "smartMeterMappings.json");
        private Dictionary<string, string> _smartMeterToPricePlanMappings;

        public SmartMeterPricePlanRepository()
        {
            _smartMeterToPricePlanMappings = JsonReader.LoadJson<Dictionary<string, string>>(_jsonFilePath) ?? new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetSmartMeterToPricePlanMappings() => _smartMeterToPricePlanMappings;
    }
}
