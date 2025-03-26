using JOIEnergy.Domain.Helpers;
using JOIEnergy.Domain.Models;
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
