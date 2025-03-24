using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOIEnergy.Domain.Models;
using JOIEnergy.Repository.Implementations.Utilities;
using JOIEnergy.Repository.Interfaces;

namespace JOIEnergy.Repository.Implementations
{
    public class PricePlanRepository : IPricePlanRepository
    {
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "pricePlans.json");
        private List<PricePlan> _pricePlans;

        public PricePlanRepository()
        {
            _pricePlans = JsonReader.LoadJson<List<PricePlan>>(_jsonFilePath) ?? new List<PricePlan>();
        }

        public List<PricePlan> GetPricePlans() => _pricePlans;
    }
}
